using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Pb.Wechat
{
    public class FtpHelper
    {
        #region 私有变量
        string ftpServerIP;
        string ftpRemotePath;
        string ftpUserID;
        string ftpPassword;
        string ftpURL;
        #endregion

        #region 构造函数
        /// <summary> 
        /// 连接FTP 
        /// </summary> 
        /// <param name="FtpServerIP">FTP连接地址</param> 
        /// <param name="FtpRemotePath">指定FTP连接成功后的当前目录, 如果不指定即默认为根目录</param> 
        /// <param name="FtpUserID">用户名</param> 
        /// <param name="FtpPassword">密码</param> 
        public FtpHelper(string FtpServerIP, string FtpRemotePath, string FtpUserID, string FtpPassword)
        {
            ftpServerIP = FtpServerIP;
            ftpRemotePath = FtpRemotePath;
            ftpUserID = FtpUserID;
            ftpPassword = FtpPassword;
            ftpURL = "ftp://" + ftpServerIP + "/" + ftpRemotePath + "/";
        }
        #endregion

        #region 上传
        /// <summary> 
        /// 上传 
        /// </summary> 
        /// <param name="ftpURL">ftp地址</param> 
        /// <param name="ftpUserID">ftp用户名</param> 
        /// <param name="ftpPassword">ftp密码</param> 
        /// <param name="bytes">文件</param> 
        /// <param name="fileName">文件名</param> 
        public static async Task Upload(string ftpURL, string ftpUserID, string ftpPassword, byte[] bytes, string fileName)
        {
            FtpWebRequest reqFTP;

            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURL + fileName));
            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
            reqFTP.KeepAlive = false;
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            reqFTP.UseBinary = true;
            reqFTP.ContentLength = bytes.LongLength;
            reqFTP.UsePassive = false;
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            long contentindex = 0 - buffLength;
            int len = 0;
            try
            {
                Stream strm = reqFTP.GetRequestStream();
                contentindex += buffLength;
                len = bytes.LongLength - contentindex >= buffLength ? buffLength : (int)(bytes.LongLength - contentindex);
                while (contentindex < bytes.LongLength)
                {
                    Array.Copy(bytes, contentindex, buff, 0, len);
                    await strm.WriteAsync(buff, 0, len);
                    contentindex += buffLength;
                    len = bytes.LongLength - contentindex >= buffLength ? buffLength : (int)(bytes.LongLength - contentindex);
                }
                strm.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary> 
        /// 上传 
        /// </summary> 
        /// <param name="fileName">文件本地路径</param> 
        /// <param name="ftpUrl">ftp相对路径</param> 
        public void Upload(string fileName, string ftpUrl)
        {
            FileInfo fileInf = new FileInfo(fileName);
            FtpWebRequest reqFTP;

            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURL + ftpUrl));
            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
            reqFTP.KeepAlive = false;
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            reqFTP.UseBinary = true;
            reqFTP.ContentLength = fileInf.Length;
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;
            FileStream fs = fileInf.OpenRead();
            try
            {
                Stream strm = reqFTP.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                strm.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 下载
        /// <summary> 
        /// 下载 
        /// </summary> 
        /// <param name="filePath">文件本地路径</param> 
        /// <param name="ftpUrl">ftp相对路径</param> 
        public void Download(string filePath, string ftpUrl)
        {
            FtpWebRequest reqFTP;
            try
            {
                FileStream outputStream = new FileStream(filePath, FileMode.Create);

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURL + ftpUrl));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 删除文件
        /// <summary> 
        /// 删除文件 
        /// </summary> 
        /// <param name="ftpFileUrl">ftp文件相对路径</param> 
        public void Delete(string ftpFileUrl)
        {
            try
            {
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURL + ftpFileUrl));

                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;

                string result = String.Empty;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                long size = response.ContentLength;
                Stream datastream = response.GetResponseStream();
                StreamReader sr = new StreamReader(datastream);
                result = sr.ReadToEnd();
                sr.Close();
                datastream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 删除文件夹
        /// <summary> 
        /// 删除文件夹 
        /// </summary> 
        /// <param name="ftpFolderUrl">ftp文件夹相对路径</param> 
        public void RemoveDirectory(string ftpFolderUrl)
        {
            try
            {
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURL + ftpFolderUrl));

                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.RemoveDirectory;

                string result = String.Empty;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                long size = response.ContentLength;
                Stream datastream = response.GetResponseStream();
                StreamReader sr = new StreamReader(datastream);
                result = sr.ReadToEnd();
                sr.Close();
                datastream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 获取当前目录下明细(包含文件和文件夹)

        /// <summary> 
        /// 获取当前目录下明细(包含文件和文件夹) 
        /// </summary> 
        /// <returns></returns> 
        public static string[] GetFilesDetailList(string ftpURL, string ftpUserID, string ftpPassword)
        {
            try
            {
                StringBuilder result = new StringBuilder();
                FtpWebRequest ftp;
                ftp = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURL));
                ftp.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                ftp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                ftp.UsePassive = false;
                WebResponse response = ftp.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);

                string line = reader.ReadLine();

                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                result.Remove(result.ToString().LastIndexOf("\n"), 1);
                reader.Close();
                response.Close();
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary> 
        /// 获取当前目录下明细(包含文件和文件夹) 
        /// </summary> 
        /// <returns></returns> 
        public string[] GetFilesDetailList()
        {
            try
            {
                StringBuilder result = new StringBuilder();
                FtpWebRequest ftp;
                ftp = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURL));
                ftp.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                ftp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                WebResponse response = ftp.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);

                string line = reader.ReadLine();

                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                result.Remove(result.ToString().LastIndexOf("\n"), 1);
                reader.Close();
                response.Close();
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 获取当前目录下文件列表(仅文件)
        /// <summary> 
        /// 获取当前目录下文件列表(仅文件) 
        /// </summary> 
        /// <returns></returns> 
        public string[] GetFileList(string mask)
        {
            StringBuilder result = new StringBuilder();
            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURL));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);

                string line = reader.ReadLine();
                while (line != null)
                {
                    if (mask.Trim() != string.Empty && mask.Trim() != "*.*")
                    {

                        string mask_ = mask.Substring(0, mask.IndexOf("*"));
                        if (line.Substring(0, mask_.Length) == mask_)
                        {
                            result.Append(line);
                            result.Append("\n");
                        }
                    }
                    else
                    {
                        result.Append(line);
                        result.Append("\n");
                    }
                    line = reader.ReadLine();
                }
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                reader.Close();
                response.Close();
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 获取当前目录下所有的文件夹列表(仅文件夹)
        /// <summary> 
        /// 获取当前目录下所有的文件夹列表(仅文件夹) 
        /// </summary> 
        /// <returns></returns> 
        public static string[] GetDirectoryList(string ftpURL, string ftpUserID, string ftpPassword)
        {
            string[] drectory = GetFilesDetailList(ftpURL, ftpUserID, ftpPassword);
            string m = string.Empty;
            foreach (string str in drectory)
            {
                int dirPos = str.IndexOf("<DIR>");
                if (dirPos > 0)
                {
                    /*判断 Windows 风格*/
                    m += str.Substring(dirPos + 5).Trim() + "\n";
                }
                else if (str.Trim().Substring(0, 5).ToLower() == "drwxr")
                {
                    /*判断 filezilla 风格*/
                    string dir = str.Substring(49).Trim();
                    if (dir != "." && dir != "..")
                    {
                        m += dir + "\n";
                    }
                }
                else if (str.Trim().Substring(0, 1).ToUpper() == "D")
                {
                    /*判断 Unix 风格*/
                    string dir = str.Substring(54).Trim();
                    if (dir != "." && dir != "..")
                    {
                        m += dir + "\n";
                    }
                }
            }

            char[] n = new char[] { '\n' };
            return m.Split(n);
        }
        /// <summary> 
        /// 获取当前目录下所有的文件夹列表(仅文件夹) 
        /// </summary> 
        /// <returns></returns> 
        public string[] GetDirectoryList()
        {
            string[] drectory = GetFilesDetailList();
            string m = string.Empty;
            foreach (string str in drectory)
            {
                int dirPos = str.IndexOf("<DIR>");
                if (dirPos > 0)
                {
                    /*判断 Windows 风格*/
                    m += str.Substring(dirPos + 5).Trim() + "\n";
                }
                else if (str.Trim().Substring(0, 1).ToUpper() == "D")
                {
                    /*判断 Unix 风格*/
                    string dir = str.Substring(54).Trim();
                    if (dir != "." && dir != "..")
                    {
                        m += dir + "\n";
                    }
                }
            }

            char[] n = new char[] { '\n' };
            return m.Split(n);
        }
        #endregion

        #region 判断当前目录下指定的子目录是否存在
        /// <summary> 
        /// 判断当前目录下指定的子目录是否存在 
        /// </summary> 
        /// <param name="RemoteDirectoryName">指定的目录名</param> 
        public static bool DirectoryExist(string ftpURL, string ftpUserID, string ftpPassword,string RemoteDirectoryName)
        {
            string[] dirList = GetDirectoryList(ftpURL, ftpUserID, ftpPassword);
            foreach (string str in dirList)
            {
                if (str.Trim() == RemoteDirectoryName.Trim())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary> 
        /// 判断当前目录下指定的子目录是否存在 
        /// </summary> 
        /// <param name="RemoteDirectoryName">指定的目录名</param> 
        public bool DirectoryExist(string RemoteDirectoryName)
        {
            string[] dirList = GetDirectoryList();
            foreach (string str in dirList)
            {
                if (str.Trim() == RemoteDirectoryName.Trim())
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 判断当前目录下指定的文件是否存在
        /// <summary> 
        /// 判断当前目录下指定的文件是否存在 
        /// </summary> 
        /// <param name="RemoteFileName">远程文件名</param> 
        public bool FileExist(string RemoteFileName)
        {
            string[] fileList = GetFileList("*.*");
            foreach (string str in fileList)
            {
                if (str.Trim() == RemoteFileName.Trim())
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 创建文件夹
        /// <summary> 
        /// 创建文件夹 
        /// </summary> 
        /// <param name="ftpURL">ftp地址</param> 
        /// <param name="ftpUserID">ftp用户名</param> 
        /// <param name="ftpPassword">ftp密码</param> 
        /// <param name="dirName"></param> 
        public static void MakeDir(string ftpURL, string ftpUserID, string ftpPassword, string dirName)
        {
            FtpWebRequest reqFTP;
            try
            {
                // dirName = name of the directory to create. 
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURL + dirName));
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.UsePassive = false;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();

                ftpStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary> 
        /// 创建文件夹 
        /// </summary> 
        /// <param name="dirName"></param> 
        public void MakeDir(string dirName)
        {
            FtpWebRequest reqFTP;
            try
            {
                // dirName = name of the directory to create. 
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURL + dirName));
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();

                ftpStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 获取指定文件大小
        /// <summary> 
        /// 获取指定文件大小 
        /// </summary> 
        /// <param name="filename"></param> 
        /// <returns></returns> 
        public long GetFileSize(string filename)
        {
            FtpWebRequest reqFTP;
            long fileSize = 0;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURL + filename));
                reqFTP.Method = WebRequestMethods.Ftp.GetFileSize;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                fileSize = response.ContentLength;

                ftpStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return fileSize;
        }
        #endregion

        #region 文件改名
        /// <summary> 
        /// 文件改名 
        /// </summary> 
        /// <param name="currentFilename"></param> 
        /// <param name="newFilename"></param> 
        public void ReName(string currentFilename, string newFilename)
        {
            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURL + currentFilename));
                reqFTP.Method = WebRequestMethods.Ftp.Rename;
                reqFTP.RenameTo = newFilename;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();

                ftpStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 移动文件
        /// <summary> 
        /// 移动文件 
        /// </summary> 
        /// <param name="currentFilename"></param> 
        /// <param name="newFilename"></param> 
        public void MovieFile(string currentFilename, string newDirectory)
        {
            ReName(currentFilename, newDirectory);
        }
        #endregion

        #region 切换当前目录
        /// <summary> 
        /// 切换当前目录 
        /// </summary> 
        /// <param name="DirectoryName"></param> 
        /// <param name="IsRoot">true 绝对路径   false 相对路径</param> 
        public void GotoDirectory(string DirectoryName, bool IsRoot)
        {
            if (IsRoot)
            {
                ftpRemotePath = DirectoryName;
            }
            else
            {
                ftpRemotePath += DirectoryName + "/";
            }
            ftpURL = "ftp://" + ftpServerIP + "/" + ftpRemotePath + "/";
        }
        #endregion

        #region 删除目录
        /// <summary> 
        /// 删除目录 
        /// </summary> 
        /// <param name="ftpServerIP">FTP 主机地址</param> 
        /// <param name="folderToDelete">FTP 用户名</param> 
        /// <param name="ftpUserID">FTP 用户名</param> 
        /// <param name="ftpPassword">FTP 密码</param> 
        public static void DeleteOrderDirectory(string ftpServerIP, string folderToDelete, string ftpUserID, string ftpPassword)
        {
            try
            {
                if (!string.IsNullOrEmpty(ftpServerIP) && !string.IsNullOrEmpty(folderToDelete) && !string.IsNullOrEmpty(ftpUserID) && !string.IsNullOrEmpty(ftpPassword))
                {
                    FtpHelper fw = new FtpHelper(ftpServerIP, folderToDelete, ftpUserID, ftpPassword);
                    //进入目录 
                    fw.GotoDirectory(folderToDelete, true);
                    //获取规格目录 
                    string[] folders = fw.GetDirectoryList();
                    foreach (string folder in folders)
                    {
                        if (!string.IsNullOrEmpty(folder) || folder != "")
                        {
                            //进入目录 
                            string subFolder = folderToDelete + "/" + folder;
                            fw.GotoDirectory(subFolder, true);
                            //获取文件列表 
                            string[] files = fw.GetFileList("*.*");
                            if (files != null)
                            {
                                //删除文件 
                                foreach (string file in files)
                                {
                                    fw.Delete(file);
                                }
                            }
                            //删除文件夹 
                            fw.GotoDirectory(folderToDelete, true);
                            fw.RemoveDirectory(folder);
                        }
                    }

                    //删除文件夹 
                    string parentFolder = folderToDelete.Remove(folderToDelete.LastIndexOf('/'));
                    string orderFolder = folderToDelete.Substring(folderToDelete.LastIndexOf('/') + 1);
                    fw.GotoDirectory(parentFolder, true);
                    fw.RemoveDirectory(orderFolder);
                }
                else
                {
                    throw new Exception("FTP 及路径不能为空！");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("删除目录时发生错误，错误信息为：" + ex.Message);
            }
        }
        #endregion
    }
}
