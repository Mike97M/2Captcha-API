# 2Captcha
C# wrapper for 2Captcha API


# Prerequisites:
2Captcha API key

# Usage:
## Get Account Balance
```
TwoCaptchaApi 2captcha = new TwoCaptchaApi(<API_KEY>);
float balance = 2captcha.getBalance();

```

## Solve Captcha
```
TwoCaptchaApi 2captcha = new TwoCaptchaApi(<API_KEY>);
string result = api.solveCaptcha(SITE_KEY,SITE_URL);
```

## Solve ReCaptcha
```
TwoCaptchaApi 2captcha = new TwoCaptchaApi(<API_KEY>);
string result = api.solveReCaptcha(SITE_KEY,SITE_URL);
```


## Report incorrect captcha
```
TwoCaptchaApi 2captcha = new TwoCaptchaApi(<API_KEY>);
float balance = 2captcha.reportBadCaptcha(2captcha.captchaId);

```



