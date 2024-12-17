# CryptoBot

A C# cryptocurrency console bot that consumes the ```ByBit``` API and shows the top crypto coins with positive changes, sending email notifications when there’s a significant change. 

```This is for study purposes only. Do not use it, or you may lose money.```

Check the API docs at [ByBit API Documentation](https://bybit-exchange.github.io/docs/v5/intro).

![image](https://github.com/user-attachments/assets/e20b2b77-9a37-4eb3-a042-bae86f224952)

## Features

- Monitors the top 10 cryptocurrencies with the highest 24-hour value increase or recent increase (configurable).
- Displays real-time information on the screen, updating every second.
- Sends an email every minute if there’s a significant change in the cryptocurrency value.

![image](https://github.com/user-attachments/assets/3617c23d-cf1a-4fac-946b-eb0d7a35c141)

## Configuring

To configure the bot, fill out the `settings.json` file as follows:

```json
{
  "apiKey": "BY_BIT_API_KEY",
  "apiSecret": "BY_BIT_SECRET_KEY",
  "email": "SENDER_EMAIL",
  "emailSecret": "SENDER_EMAIL_SECRET",
  "smtp": "SMTP_SERVER",
  "smtpPort": 587
}
```

To create your keys, visit [ByBit API Management](https://www.bybit.com/app/user/api-management). Be sure to set the API to read-only and restrict access to your IP address.

Additionally, in the ```Program.cs``` file, you can modify the bot's parameters such as:

* ```production```: Set to true to send to the production environment; false to use the test environment.
* ```emailsToSend```: Emails that will receive the coin status updates.
* ```percentage24h```: The minimum percentage increase in the last 24 hours to display coins.
* ```percentageIncrease```: The minimum percentage increase since the program started to display coins.

## Changing email trigger

The following parameters are hard-coded. To change the email trigger conditions, go to ```CheckCoinIncreaseBot.cs``` in the ```SendEmail``` function:

```C#
 bool worthSending = results.Any(x =>
     x.PctBeg > 10.0m ||
     x.Pct5M > 15.0m ||
     x.Pct30M > 20.0m
 );
```
