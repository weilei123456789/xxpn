using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Penny
{
    public static class HttpUploadFileHelper
    {
        /// <summary>
        /// HttpUploadFile
        /// </summary>
        /// <param name="url"></param>
        /// <param name="bytes"></param>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string HttpUploadFile(string url, byte[] bytes, string fileName, Dictionary<string, string> data)
        {
            string boundary = "----WebKitFormBoundary" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
            byte[] endbytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");

            //1.HttpWebRequest
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = "POST";
            request.KeepAlive = true;
            request.Credentials = CredentialCache.DefaultCredentials;

            using (Stream stream = request.GetRequestStream())
            {
                //1.1 key/value
                string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                if (data != null)
                {
                    foreach (string key in data.Keys)
                    {
                        stream.Write(boundarybytes, 0, boundarybytes.Length);
                        string formitem = string.Format(formdataTemplate, key, data[key]);
                        byte[] formitembytes = Encoding.UTF8.GetBytes(formitem);
                        stream.Write(formitembytes, 0, formitembytes.Length);
                    }
                }

                //1.2 file
                stream.Write(boundarybytes, 0, boundarybytes.Length);
                string header = string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n", "file", fileName);
                byte[] headerbytes = Encoding.UTF8.GetBytes(header);
                stream.Write(headerbytes, 0, headerbytes.Length);
                stream.Write(bytes, 0, bytes.Length);

                //1.3 form end
                stream.Write(endbytes, 0, endbytes.Length);
            }
            //2.WebResponse
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                return stream.ReadToEnd();
            }
        }
    }
}
