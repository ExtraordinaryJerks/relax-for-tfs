!include x64.nsh
!include WordFunc.nsh
!insertmacro VersionCompare
!include LogicLib.nsh

SetCompressor /FINAL /SOLID lzma
SetCompressorDictSize 64

# define installer name
OutFile "RelaxForTFSInstaller.exe"

# set desktop as install directory
InstallDir "$PROGRAMFILES\Relax For TFS"

Var FriendlyName
Var ShortName
Var UninstallPath
Var UninstallRegKey
Var StartMenuFolder
Var StartMenuUninstall
 
# For removing Start Menu shortcut in Windows 7
RequestExecutionLevel admin

Function .onInit
	#Mutually exclusive
	System::Call 'kernel32::CreateMutexA(i 0, i 0, t "myMutex") i .r1 ?e'
	Pop $R0
	 
	StrCmp $R0 0 +3
	MessageBox MB_OK|MB_ICONEXCLAMATION "The installer is already running."
	Abort
			
	#check for DotNet version 4.0
	Call CheckAndInstallDotNet
	Pop $0
	#${If} $0 == "not found"
	#	MessageBox MB_OK|MB_ICONSTOP ".NET runtime library is not installed."
	#	Abort
	#${EndIf}
	 
	#StrCpy $0 $0 "" 1 # skip "v"
	 
	#${VersionCompare} $0 "4.0.0.0" $1
	#${If} $1 == 2
	#	MessageBox MB_OK|MB_ICONSTOP ".NET runtime library v4.0 or newer is required. You have $0."
	#	Abort
	#${EndIf}
	
	UserInfo::GetAccountType
	pop $0
	${If} $0 != "admin" ;Require admin rights on NT4+
		MessageBox mb_iconstop "Administrator rights required!"
		SetErrorLevel 740 ;ERROR_ELEVATION_REQUIRED
		Quit
	${EndIf}
	
	SetShellVarContext all
	
	StrCpy $FriendlyName "Relax For TFS"
	StrCpy $ShortName "RelaxForTFS"
	StrCpy $UninstallPath "$INSTDIR\uninstaller.exe"
	StrCpy $UninstallRegKey "Software\Microsoft\Windows\CurrentVersion\Uninstall\RelaxForTFS"
	StrCpy $StartMenuFolder "$SMPROGRAMS\$FriendlyName"
	StrCpy $StartMenuUninstall "$StartMenuFolder\Uninstall Relax For TFS.lnk"
	
	Call CheckForActiveInstallAndUninstall
 
FunctionEnd

Function un.onInit
	SetShellVarContext all
	
	StrCpy $FriendlyName "Relax For TFS"
	StrCpy $ShortName "RelaxForTFS"
	StrCpy $UninstallPath "$INSTDIR\uninstaller.exe"
	StrCpy $UninstallRegKey "Software\Microsoft\Windows\CurrentVersion\Uninstall\RelaxForTFS"
	StrCpy $StartMenuFolder "$SMPROGRAMS\$FriendlyName"
	StrCpy $StartMenuUninstall "$StartMenuFolder\Uninstall Relax For TFS.lnk"
FunctionEnd

# default section start
section
	SetShellVarContext all
	# define output path
	SetOutPath "$INSTDIR"
	 
	# specify file to go in output path
	File "..\Relax.WindowsService\bin\Debug\*.*"
	File "c:\Windows\Microsoft.NET\Framework64\v4.0.30319\InstallUtil.exe"

	# define uninstaller name
	WriteUninstaller "$UninstallPath"
	
	# create a shortcut named "new shortcut" in the start menu programs directory
    # point the new shortcut at the program uninstaller
	CreateDirectory "$StartMenuFolder"
    CreateShortCut "$StartMenuUninstall" "$UninstallPath"
	
	WriteRegStr HKLM "$UninstallRegKey" "DisplayName" $FriendlyName
	WriteRegStr HKLM "$UninstallRegKey" "Publisher" "Threeguyz"
	WriteRegStr HKLM "$UninstallRegKey" "UninstallString" "$\"$UninstallPath$\""

	ExecWait '"$INSTDIR\InstallUtil.exe" /name="$ShortName" "$INSTDIR\Relax.exe"'
	ExecWait '"sc.exe" start "$FriendlyName"'
	
#-------
# default section end
sectionEnd

# create a section to define what the uninstaller does.
# the section will always be named "Uninstall"
section "Uninstall"
	SetShellVarContext all
	
	ExecWait '"$INSTDIR\InstallUtil.exe" /u "$INSTDIR\Relax.exe"'
		
	# Always delete uninstaller first
	Delete "$UninstallPath"
	
	# now delete installed file
	Delete "$INSTDIR\*.*"
	RMDir "$INSTDIR"
			
	# second, remove the link from the start menu
    Delete "$StartMenuUninstall"
	RMDir "$StartMenuFolder"
	
	DeleteRegKey HKLM "$UninstallRegKey"
 
sectionEnd

Function CheckAndInstallDotNet
    ; Magic numbers from http://msdn.microsoft.com/en-us/library/ee942965.aspx
    ClearErrors
    ReadRegDWORD $0 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" "Release"

    IfErrors NotDetected

    ${If} $0 >= 378389
        DetailPrint "Microsoft .NET Framework 4.5 is installed ($0)"
    ${Else}
    NotDetected:
        DetailPrint "Installing Microsoft .NET Framework 4.5"
		MessageBox MB_OK "You must have Microsoft .NET Framework 4.5 to install."
		Abort
        #SetDetailsPrint listonly
        #ExecWait '"$INSTDIR\Tools\dotNetFx45_Full_setup.exe" /passive /norestart' $0
        #${If} $0 == 3010 
        #${OrIf} $0 == 1641
        #    DetailPrint "Microsoft .NET Framework 4.5 installer requested reboot"
        #    SetRebootFlag true
        #${EndIf}
        #SetDetailsPrint lastused
        #DetailPrint "Microsoft .NET Framework 4.5 installer returned $0"
    ${EndIf}

FunctionEnd

Function CheckForActiveInstallAndUninstall
	ReadRegStr $R0 HKLM "$UninstallRegKey" "UninstallString"
	StrCmp $R0 "" done

	MessageBox MB_OKCANCEL|MB_ICONEXCLAMATION \
	"$FriendlyName is already installed. $\n$\nClick `OK` to remove the \
	previous version or `Cancel` to cancel this upgrade." \
	IDOK uninst
	Abort

	;Run the uninstaller
	uninst:
		ClearErrors
		ExecWait '$R0 _?=$INSTDIR' ;Do not copy the uninstaller to a temp file

		IfErrors no_remove_uninstaller done
		;You can either use Delete /REBOOTOK in the uninstaller or add some code
		;here to remove the uninstaller. Use a registry key to check
		;whether the user has chosen to uninstall. If you are using an uninstaller
		;components page, make sure all sections are uninstalled.
		no_remove_uninstaller:
	done:
FunctionEnd
