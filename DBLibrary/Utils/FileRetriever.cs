using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using System.Reflection;

namespace Utils
{
    public class FileRetrieverFactory
    {
        public static FileRetriever newInstance()
        {
            return new FileRetrieverImpl();
        }
    }

    public interface FileRetriever
    {
        byte[] RetrieveBytes(String aFile);
        byte[] RetrieveBytesFromHttp(String aFile);
        byte[] RetrieveBytesFromPhysicalFilePath(String aFile);
        byte[] RetrieveBytesFromVirtualFilePath(String aFile);
        byte[] RetrieveBytesFromAssembly(String aFile);


        Stream RetrieveStream(String aFile);
        Stream RetrieveStreamFromHttp(String aFile);
        Stream RetrieveStreamFromPhysicalFilePath(String aFile);
        Stream RetrieveStreamFromVirtualFilePath(String aFile);
        Stream RetrieveStreamFromAssembly(String aFile);

        bool isAssembly(String aFile);
        bool isVirtual(String aFile);
        bool isUri(String aFile);
        bool isPhysical(String aFile);
    }

    class FileRetrieverImpl : FileRetriever
    {
        private HttpServerUtility ServerUtility;

        public FileRetrieverImpl()
        {
            if (HttpContext.Current != null)
                ServerUtility = HttpContext.Current.Server;
        }

        #region Retrieve Bytes

        public byte[] RetrieveBytesFromHttp(String aFile)
        {
            using (WebClient client = new WebClient())
            {
                return client.DownloadData(new Uri(aFile));
            }
        }

        public byte[] RetrieveBytesFromAssembly(String aFile)
        {
            using (Stream _stream = RetrieveStreamFromAssembly(aFile))
            {
                if (_stream != null && !_stream.CanRead)
                {
                    throw new ArgumentException();
                }

                _stream.Seek(0, SeekOrigin.Begin);

                byte[] output = new byte[_stream.Length];
                int bytesRead = _stream.Read(output, 0, output.Length);
                return output;
            }
        }

        public byte[] RetrieveBytesFromPhysicalFilePath(String aFile)
        {
            return File.ReadAllBytes(aFile);
        }

        public byte[] RetrieveBytesFromVirtualFilePath(String aFile)
        {
            return RetrieveBytesFromPhysicalFilePath(ServerUtility.MapPath(aFile));
        }

        public byte[] RetrieveBytes(String aFile)
        {
            return Retrieve<byte[]>(aFile, RetrieveBytesFromHttp,
               RetrieveBytesFromAssembly, RetrieveBytesFromPhysicalFilePath, RetrieveBytesFromVirtualFilePath);
        }
        #endregion

        #region Retrieve Streams
        public Stream RetrieveStream(string aFile)
        {
            return Retrieve<Stream>(aFile, RetrieveStreamFromHttp,
                RetrieveStreamFromAssembly, RetrieveStreamFromPhysicalFilePath, RetrieveStreamFromVirtualFilePath);
        }

        public Stream RetrieveStreamFromHttp(String aFile)
        {
            return new MemoryStream(RetrieveBytesFromHttp(aFile));
        }

        public Stream RetrieveStreamFromAssembly(string aFile)
        {
            String _file = aFile.Substring(1, aFile.Length - 1);
            foreach (Assembly _assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Stream _stream = _assembly.GetManifestResourceStream(_file);
                if (_stream != null)
                {
                    return _stream;
                }
            }
            return null;
        }

        public Stream RetrieveStreamFromPhysicalFilePath(string aFile)
        {
            return File.Open(aFile, FileMode.Open, FileAccess.Read,FileShare.Read);
        }

        public Stream RetrieveStreamFromVirtualFilePath(string aFile)
        {
            return RetrieveStreamFromPhysicalFilePath(ServerUtility.MapPath(aFile));
        }
        #endregion

        #region check
        public bool isAssembly(string aFile)
        {
            return aFile.StartsWith(":"); 
        }

        public bool isVirtual(string aFile)
        {
            throw new NotImplementedException();
        }

        public bool isUri(string aFile)
        {
            return aFile.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase);
        }

        public bool isPhysical(string aFile)
        {
            return ((aFile.Length > 2 && aFile[1] == ':') || ServerUtility == null);
        }
        #endregion

        #region Private
        private delegate T RetrieveDelegate<T>(String aFile);
        
        private T Retrieve<T>(String aFile,
                            RetrieveDelegate<T> aHttpRetrieveDelegate,
                            RetrieveDelegate<T> aEmbededRetrieveDelegate,
                            RetrieveDelegate<T> aPhysicalRetrieveDelegate,
                            RetrieveDelegate<T> aVirtualRetrieveDelegate)
            where T : class
        {
            if (isUri(aFile))
            {
                return aHttpRetrieveDelegate(aFile);
            }
            else if (isAssembly(aFile))
            {
                return aEmbededRetrieveDelegate(aFile);
            }
            else if (isPhysical(aFile))
            {
                return aPhysicalRetrieveDelegate(aFile);
            }
            else 
            {
                return aVirtualRetrieveDelegate(aFile);
            }
        }
        #endregion


    }
}
