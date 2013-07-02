using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Relax.Model.Services
{
    public class MultipartParser
    {
        private byte[] _requestData;
        public MultipartParser(Stream stream)
        {
            Parse(stream, Encoding.UTF8);
            ParseParameter(stream, Encoding.UTF8);
        }

        public MultipartParser(Stream stream, Encoding encoding)
        {
            Parse(stream, encoding);
        }

        private void Parse(Stream stream, Encoding encoding)
        {
            Success = false;

            // Read the stream into a byte array
            var data = ToByteArray(stream);
            _requestData = data;

            // Copy to a string for header parsing
            var content = encoding.GetString(data);

            // The first line should contain the delimiter
            var delimiterEndIndex = content.IndexOf("\r\n", StringComparison.Ordinal);

            if (delimiterEndIndex <= -1) return;

            var delimiter = content.Substring(0, content.IndexOf("\r\n", StringComparison.Ordinal));

            // Look for Content-Type
            var re = new Regex(@"(?<=Content\-Type:)(.*?)(?=\r\n\r\n)");
            var contentTypeMatch = re.Match(content);

            // Look for filename
            re = new Regex(@"(?<=filename\=\"")(.*?)(?=\"")");
            var filenameMatch = re.Match(content);

            // Did we find the required values?
            if (contentTypeMatch.Success && filenameMatch.Success)
            {
                // Set properties
                ContentType = contentTypeMatch.Value.Trim();
                Filename = filenameMatch.Value.Trim();

                // Get the start & end indexes of the file contents
                var startIndex = contentTypeMatch.Index + contentTypeMatch.Length + "\r\n\r\n".Length;

                var delimiterBytes = encoding.GetBytes("\r\n" + delimiter);
                var endIndex = IndexOf(data, delimiterBytes, startIndex);

                var contentLength = endIndex - startIndex;

                // Extract the file contents from the byte array
                var fileData = new byte[contentLength];

                Buffer.BlockCopy(data, startIndex, fileData, 0, contentLength);

                FileContents = fileData;
                Success = true;
            }
        }

        private void ParseParameter(Stream stream, Encoding encoding)
        {
            Success = false;

            // Read the stream into a byte array
            byte[] data = _requestData.Length == 0 ? ToByteArray(stream) : _requestData;
            // Copy to a string for header parsing
            var content = encoding.GetString(data);

            // The first line should contain the delimiter
            var delimiterEndIndex = content.IndexOf("\r\n", StringComparison.Ordinal);

            if (delimiterEndIndex > -1)
            {
                var delimiter = content.Substring(0, content.IndexOf("\r\n", StringComparison.Ordinal));
                var splitContents = content.Split(new[] {delimiter}, StringSplitOptions.RemoveEmptyEntries);
                foreach (var t in splitContents)
                {
                    // Look for Content-Type
                    var contentTypeRegex = new Regex(@"(?<=Content\-Type:)(.*?)(?=\r\n\r\n)");
                    var contentTypeMatch = contentTypeRegex.Match(t);

                    // Look for name of parameter
                    var re = new Regex(@"(?<=name\=\"")(.*)((?=\"";)|(?=\""\r\n\r\n))");
                    var name = re.Match(t);

                    // Look for filename
                    re = new Regex(@"(?<=filename\=\"")(.*?)(?=\"")");
                    var filenameMatch = re.Match(t);

                    // Did we find the required values?
                    if (name.Success || filenameMatch.Success)
                    {
                        // Set properties
                        //this.ContentType = name.Value.Trim();
                        int startIndex;
                        if (filenameMatch.Success)
                        {
                            Filename = filenameMatch.Value.Trim();
                        }
                        if(contentTypeMatch.Success)
                        {
                            // Get the start & end indexes of the file contents
                            startIndex = contentTypeMatch.Index + contentTypeMatch.Length + "\r\n\r\n".Length;
                        }
                        else
                        {
                            startIndex = name.Index + name.Length + "\r\n\r\n".Length;
                        }

                        //byte[] delimiterBytes = encoding.GetBytes("\r\n" + delimiter);
                        //int endIndex = IndexOf(data, delimiterBytes, startIndex);

                        //int contentLength = t.Length - startIndex;
                        string propertyData = t.Substring(startIndex - 1, t.Length - startIndex);
                        // Extract the file contents from the byte array
                        //byte[] paramData = new byte[contentLength];

                        //Buffer.BlockCopy(data, startIndex, paramData, 0, contentLength);

                        var myContent = new Content
                                            {
                                                Data = encoding.GetBytes(propertyData),
                                                StringData = propertyData,
                                                PropertyName = name.Value.Trim()
                                            };

                        if (Contents == null)
                            Contents = new List<Content>();

                        Contents.Add(myContent);
                        Success = true;
                    }
                }
            }
        }

        private int IndexOf(byte[] searchWithin, byte[] serachFor, int startIndex)
        {
            int index = 0;
            int startPos = Array.IndexOf(searchWithin, serachFor[0], startIndex);

            if (startPos != -1)
            {
                while ((startPos + index) < searchWithin.Length)
                {
                    if (searchWithin[startPos + index] == serachFor[index])
                    {
                        index++;
                        if (index == serachFor.Length)
                        {
                            return startPos;
                        }
                    }
                    else
                    {
                        startPos = Array.IndexOf(searchWithin, serachFor[0], startPos + index);
                        if (startPos == -1)
                        {
                            return -1;
                        }
                        index = 0;
                    }
                }
            }

            return -1;
        }

        private byte[] ToByteArray(Stream stream)
        {
            var buffer = new byte[32768];
            using (var ms = new MemoryStream())
            {
                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        return ms.ToArray();
                    ms.Write(buffer, 0, read);
                }
            }
        }

        public List<Content> Contents { get; set; }

        public bool Success
        {
            get;
            private set;
        }

        public string ContentType
        {
            get;
            private set;
        }

        public string Filename
        {
            get;
            private set;
        }

        public byte[] FileContents
        {
            get;
            private set;
        }
    }

    public class Content
    {
        public byte[] Data { get; set; }
        public string PropertyName { get; set; }
        public string StringData { get; set; }
    }
}
