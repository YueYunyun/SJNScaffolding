﻿using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SJNScaffolding.Helper
{
    public class FileHelper
    {
        /// <summary>
        /// 获取路径下所有文件以及子文件夹中文件
        /// </summary>
        /// <param name="path">全路径根目录</param>
        /// <param name="fileList">存放所有文件的全路径</param>
        /// <returns></returns>
        public static Dictionary<string, FileInfo> GetFile(string path, Dictionary<string, FileInfo> fileList)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] fil = dir.GetFiles();
            DirectoryInfo[] dii = dir.GetDirectories();
            foreach (FileInfo f in fil)
            {
                fileList.Add(f.FullName, f);//添加文件路径到列表中
            }
            //获取子文件夹内的文件列表，递归遍历
            foreach (DirectoryInfo d in dii)
            {
                GetFile(d.FullName, fileList);
            }
            return fileList;
        }
        /// <summary>
        /// 输出文件并新建文件
        /// </summary>
        /// <param name="path">全路径根目录</param>
        /// <param name="content">输出的内容</param>
        /// <returns></returns>
        public static void OutputFile(string path, string content)
        {
            using (FileStream fs = new FileStream(path: path, mode: FileMode.OpenOrCreate, access: FileAccess.ReadWrite))
            {
                StreamWriter sw = new StreamWriter(fs); //创建写入流
                sw.WriteLine(content); // 写入转换后的模板内容
                sw.Close();
                fs.Close();
            }
        }
        /// <summary>
        /// 得到文件中的内容
        /// </summary>
        /// <param name="path">全路径根目录</param>
        /// <returns></returns>
        public static async Task<string> GetFileContent(string path)
        {
            using (FileStream fs = new FileStream(path: path, mode: FileMode.Open, access: FileAccess.ReadWrite))
            {
                int fsLen = (int)fs.Length;
                byte[] heByte = new byte[fsLen];
                int r = await fs.ReadAsync(heByte, 0, heByte.Length);
                string content = System.Text.Encoding.UTF8.GetString(heByte);
                return content;
            }
        }


        public static void CreateFile(string sourePath, string fileName, string content)
        {
            string path = Path.GetTempPath();
            Directory.CreateDirectory(path);
            string file = Path.Combine(path, fileName);
            File.WriteAllText(file, content, Encoding.UTF8);
            try
            {
                if (string.IsNullOrEmpty(sourePath))
                {
                    sourePath = @"..\..\";
                }

                if (!File.Exists(sourePath))
                {
                    Directory.CreateDirectory(sourePath);
                }
                string soureUrl = Path.Combine(sourePath, fileName);
                if (File.Exists(soureUrl))
                {
                    File.Delete(soureUrl);
                }
                File.Copy(file, soureUrl);
            }
            finally
            {
                File.Delete(file);
            }
        }
    }
}
