using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace API2Captcha
{
    public class TwoCaptchaApi
    {
        string key = null;
        public string captchaId;
        public TwoCaptchaApi(string key)
        {
            this.key = key;
        }

        public float getBalance()
        {
            string response = "";

            using (WebClient client = new WebClient())
            {
                client.QueryString.Add("key", key);
                client.QueryString.Add("action", "getbalance");
                response = client.DownloadString(settings.url_response);
            }


            float balance = -1;

            if (!float.TryParse(response, out balance))
            {
                throw new Exception($"2Captcha - Error while checking balance: {response}");

            }
            return balance;



        }

        public string solveReCaptcha(string googleKey, string pageUrl)
        {
            captchaId = sendReCaptcha(googleKey, pageUrl);
            Thread.Sleep(15 * 1000);


            return getResult(captchaId);

        }

        private string sendReCaptcha(string googleKey, string pageUrl)
        {
            string response = "";
            using (WebClient client = new WebClient())
            {
                client.QueryString.Add("key", key);
                client.QueryString.Add("method", "userrecaptcha");
                client.QueryString.Add("googlekey", googleKey);
                client.QueryString.Add("pageurl", pageUrl);
                response = client.DownloadString(settings.url_request);
            }

            if (response.Substring(0, 3) != "OK|")
                throw new Exception($"Captcha sending error: {response}");

            return response.Remove(0, 3);
        }
        public string solveCaptcha(string path)
        {
            captchaId = uploadCaptcha(path);
            Thread.Sleep(10 * 1000);
            return getResult(captchaId);


        }
        private string uploadCaptcha(string path)
        {
            if (!File.Exists(path))
            {
                throw new Exception("File doesn't exist");
            }
            byte[] image = File.ReadAllBytes(path);
            string response = "";
            using (WebClient client = new WebClient())
            {

                client.QueryString.Add("key", key);
                response = Encoding.Default.GetString(client.UploadFile(settings.url_request, path));
            }

            if (response.Substring(0, 3) != "OK|")
            {
                throw new Exception($"Captcha uploading error: {response}");
            }
            return response.Remove(0, 3);




        }
        public string getResult(string captchaId)
        {
            string response = "";
            for (int i = 0; i <= 10; i++)
            {
                using (WebClient client = new WebClient())
                {
                    client.QueryString.Add("key", key);
                    client.QueryString.Add("action", "get");

                    client.QueryString.Add("id", captchaId);
                    //   string url = String.Format("{0}?key={1}&action=get&id={2}", settings.url_response, key, captchaId);
                    response = client.DownloadString(settings.url_response);
                }

                if (response.Substring(0, 3) == "OK|")
                {
                    return response.Remove(0, 3);
                }
                else if (response.Contains("ERROR"))
                {
                    throw new Exception($"Captcha solve error: {response}");
                }
                Thread.Sleep(5 * 1000);
            }


            throw new Exception($"Captcha solve error: {response}");
        }


        public bool reportBadCaptcha(string captchaId)
        {
            string response = "";
            using (WebClient client = new WebClient())
            {
                client.QueryString.Add("key", key);
                client.QueryString.Add("action", "reportbad");
                client.QueryString.Add("id", captchaId);

                response = client.DownloadString(settings.url_response);
            }

            if (response.Contains("OK_REPORT_RECORDED"))
            {
                return true;
            }
            return false;


        }

    }


    static class settings
    {
        public const string url_request = "http://2captcha.com/in.php";
        public const string url_response = "http://2captcha.com/res.php";
    }
}
