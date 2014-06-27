
/*
 * 
 * 비동기 형태로 구현하는 File IO 로봇의 형태를 구조한다.
 * SIMONDataIOCommand 포멧에 맞게 주문 정보를 입력하면 그에 대한 입출력 작업을 비동기로 처리해주는 클래스를 구현.
 * Service로 요청하고, ReadResult / WriteResult 함수를 통해서 비동기 작업 결과를 저장한다.
 * 
 * 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace SIMONFramework
{
    /// <summary>
    /// SIMONFramework에서 사용하는 File입출력에 대한 열거형 타입을 정의합니다.
    /// </summary>
    public enum SIMONDataIO { READ = 0, WRITE = 1 };        //데이터 입출력에 대한 타입 분류.
    
    /// <summary>
    /// SIMONDataIO에 대한 실제적인 명령 포멧을 정의합니다.
    /// </summary>
    public struct SIMONDataIOCommand
    {
        //Data IO를 위한 명령에 대한 포맷을 지정한다.
        public SIMONDataIO order;
        public string fileName;
        public string contents;
    }

    /// <summary>
    /// 데이터에 대한 비동기 입출력을 지원하는 클래스입니다.
    /// </summary>
    public class SIMONDataManager
    {
        //데이터에 대한 비동기 입출력을 지원하는 클래스.
        public delegate string AsyncReader(string fileName, ref bool error, ref int lineCount);                     //비동기 입력 델리게이트.
        public delegate void AsyncWriter(string fileName, string contents, ref bool error);                         //비동기 출력 델리게이트.
        public static AsyncReader AsyncRead { get; set; }                                                           //비동기 입력 함수포인터.
        public static AsyncWriter AsyncWrite { get; set; }                                                          //비동기 출력 함수포인터.

        public SIMONDataManager()
        {
            AsyncRead = new AsyncReader(Read);
            AsyncWrite = new AsyncWriter(Write);
        }
        private string Read(string fileName, ref bool error, ref int lineCount)
        {
            //fileName으로부터 모든 스트링 컨텐츠를 읽고 예외 상황을 error 참조 변수에 저장한다.
            string text = "";
            try
            {
                FileStream fStream = new FileStream(fileName, FileMode.Open);
                StreamReader sReader = new StreamReader(fStream);
                //text = sReader.ReadToEnd();
                while (!sReader.EndOfStream)
                {
                    text += sReader.ReadLine();
                    lineCount++;
                }

                sReader.Close();
                fStream.Close();
            }
            catch (System.IO.IOException e)
            {
                error = true;
                Console.WriteLine(e.Message);
            }
            return text;
        }
        private void Write(string fileName, string contents, ref bool error)
        {
            //fileName에 contents를 쓰고, 수행 예외 상황을 error 참조 변수에 저장한다.
            try
            {
                //FileStream에 대한 중복 Access 방지.
                if (!File.Exists(fileName))
                {
                    File.Create(fileName).Close();
                }

                FileStream fStream = new FileStream(fileName, FileMode.Append);
                StreamWriter sWriter = new StreamWriter(fStream);
                sWriter.Write(contents);
                sWriter.Close();
                fStream.Close();
            }
            catch (System.IO.IOException e)
            {
                error = true;
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// 주문을 받아서 비동기 파일 입출력에 대한 서비스를 실행해주는 함수입니다.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public IAsyncResult Service(ref SIMONDataIOCommand data)
        {
            //주문(order)을 받아서 서비스를 실행함. 서비스의 결과를 리턴해줘야함.
            bool serviceChecker = false;
            IAsyncResult serviceResult = null;
            int lineCount = 0;

            //비동기 루틴을 실행하고 AsyncResult를 반환한다.
            if (data.order.Equals(SIMONDataIO.READ))
            {
                serviceResult = AsyncRead.BeginInvoke(data.fileName, ref serviceChecker, ref lineCount, null, null);
            }
            else if (data.order.Equals(SIMONDataIO.WRITE))
            {
                serviceResult = AsyncWrite.BeginInvoke(data.fileName, data.contents, ref serviceChecker, null, null);
            }
            return serviceResult;
        }

        /// <summary>
        /// 요청한 읽기 작업에 대한 핸들을 받아 결과를 반환하는 함수입니다.
        /// </summary>
        /// <param name="errorFlag"></param>
        /// <param name="contents"></param>
        /// <param name="ar"></param>
        /// <returns></returns>
        public bool ReadResult(ref bool errorFlag, ref string contents, ref int lineCount, IAsyncResult ar)
        {
            //입력 AsyncResult에 대해서 입력 작업중 error를 errorFlag에 저장하고, 읽은 결과를 contents에 저장한다. 예외 발생시 false를 리턴.
            bool ret = true;
            try
            {
                contents = AsyncRead.EndInvoke(ref errorFlag, ref lineCount, ar);
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine(e.Message);
                ret = false;
            }
            return ret;
        }

        /// <summary>
        /// 요청한 쓰기 작업에 대한 핸들을 받아 결과를 반환하는 함수입니다.
        /// </summary>
        /// <param name="errorFlag"></param>
        /// <param name="ar"></param>
        /// <returns></returns>
        public bool WriteResult(ref bool errorFlag, IAsyncResult ar)
        {
            //출력 AsyncResult에 대한 결과를 반환한다.
            bool ret = true;
            try
            {
                AsyncWrite.EndInvoke(ref errorFlag, ar);
            }
            catch (System.IO.IOException e)
            {
                Console.WriteLine(e.Message);
                ret = false;
            }
            return ret;
        }
    }
}