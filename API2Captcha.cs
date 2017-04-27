using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace API2Captcha
{
    public class TwoCaptchaApi
    {
        private string key;
        private string captchaId;
        public TwoCaptchaApi(string key)
        {
            this.key = key;
        }

        public float GetBalance()
        {
            string response = "";
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.QueryString.Add("key", key);
                    client.QueryString.Add("action", "getbalance");
                    response = client.DownloadString(settings.url_response);

                    return float.Parse(response);
                }
            }
            catch (Exception ex)
            {

                throw new BalanceException(response, ex);
            }
        }

        public string SolveReCaptcha(string googleKey, string pageUrl)
        {
            captchaId = sendReCaptcha(googleKey, pageUrl);
            Thread.Sleep(15 * 1000);


            return getResult(captchaId);

        }

        private string sendReCaptcha(string googleKey, string pageUrl)
        {

            using (WebClient client = new WebClient())
            {
                client.QueryString.Add("key", key);
                client.QueryString.Add("method", "userrecaptcha");
                client.QueryString.Add("googlekey", googleKey);
                client.QueryString.Add("pageurl", pageUrl);
                string response = client.DownloadString(settings.url_request);
                return processResponse(response, "ReCaptcha sending error");
            }


        }
        private string processResponse(string response, string exceptionMessage)
        {
            string status = response.Split('|')[0];
            if (status == "OK")
            {
                string captchaResponse = response.Remove(0, 3);
                return captchaResponse;
            }
            else if (status == "ERROR")
            {
                throw new Exception($"{exceptionMessage}: {response}");
            }
            else if (status == "CAPCHA_NOT_READY")
            {
                return status;
            }
            throw new ResponseException($"{exceptionMessage}: {response}");
        }
        public string SolveCaptcha(string path)
        {
            captchaId = uploadCaptcha(path);
            Thread.Sleep(10 * 1000);
            return getResult(captchaId);


        }
        private string uploadCaptcha(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Image was not found.");
            }
            byte[] image = File.ReadAllBytes(path);

            using (WebClient client = new WebClient())
            {

                client.QueryString.Add("key", key);
                string response = Encoding.Default.GetString(client.UploadFile(settings.url_request, path));
                return processResponse(response, "Captcha uploading error");
            }
        }
        private string getResult(string captchaId)
        {
            string response = "";
            for (int i = 0; i <= 10; i++)
            {
                using (WebClient client = new WebClient())
                {
                    client.QueryString.Add("key", key);
                    client.QueryString.Add("action", "get");
                    client.QueryString.Add("id", captchaId);
                    response = client.DownloadString(settings.url_response);

                    string captchaResponse = processResponse(response, "Error while getting captcha response");
                    if (captchaResponse == "CAPCHA_NOT_READY")
                    {
                        Thread.Sleep(5 * 1000);
                    }
                    else
                    {
                        return captchaResponse;
                    }
                }





            }


            throw new Exception($"Captcha solve error: {response}");
        }


        public bool ReportBadCaptcha()
        {

            using (WebClient client = new WebClient())
            {
                client.QueryString.Add("key", key);
                client.QueryString.Add("action", "reportbad");
                client.QueryString.Add("id", captchaId);

                string response = client.DownloadString(settings.url_response);
                return response.Contains("OK_REPORT_RECORDED");
            }

        }

    }


    static class settings
    {
        public const string url_request = "http://2captcha.com/in.php";
        public const string url_response = "http://2captcha.com/res.php";
    }
}
