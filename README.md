# 2Captcha
C# wrapper for 2Captcha API


# Prerequisites:
2Captcha API key

# Usage:
## Get Account Balance
```
TwoCaptchaApi api = new TwoCaptchaApi(<API_KEY>);
float balance = api.getBalance();

```

## Solve Captcha
```
TwoCaptchaApi api = new TwoCaptchaApi(<API_KEY>);
string result = api.SolveCaptcha(IMAGE_PATH);
```

## Solve ReCaptcha
```
TwoCaptchaApi api = new TwoCaptchaApi(<API_KEY>);
string result = api.SolveReCaptcha(SITE_KEY,SITE_URL);
```


## Report incorrect captcha
```
TwoCaptchaApi api = new TwoCaptchaApi(<API_KEY>);
bool reported = api.reportBadCaptcha(2captcha.captchaId);

```



