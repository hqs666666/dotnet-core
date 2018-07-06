using System;
using System.Collections.Generic;
using System.Linq;
using DotNetCore.Core.Base;
using DotNetCore.Core.Base.DTOS;
using DotNetCore.Core.Base.DTOS.User;
using DotNetCore.Core.Base.Services;
using DotNetCore.Core.Base.Services.Log;
using DotNetCore.Core.Base.Services.Message;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore.Internal;
using MimeKit;

namespace DotNetCore.Core.Services.Message
{
    public class EmailService : IEmailService
    {
        private readonly ILogService mLogService;
        private readonly IConfigService mConfigService;

        public EmailService(ILogService logService, IConfigService configService)
        {
            mLogService = logService;
            mConfigService = configService;
        }

        public ResultMsg VaildEmail(UserDto user)
        {
            var lEmail = new Email { Receiver = new List<string>(){ user.Email },Subject = "账户激活邮件"};
            lEmail.Body = $"<p>尊敬的用户：{user.NickName}</p>";
            lEmail.Body += $"<p>请点击一下链接来激活您的账户：<a href='{mConfigService.AuthUrl}/api/values/active/{user.Id}'>hhhhhhhhhhhhhhh</a></p>";
            lEmail.Body += "<p>看到此邮件请不要慌张，这只是我RabbitMQ的测试而已，谢谢配合！</p>";
            lEmail.Body += "<p>该邮件为系统邮件，请勿回复</p>";
            return Send(lEmail);
        }

        public ResultMsg Send(Email email)
        {
            if (!email.Receiver.Any())
                return CreateErrorMsg("邮箱不存在！");

            var lMessage = new MimeMessage();
            lMessage.From.Add(new MailboxAddress(mConfigService.Name,mConfigService.Address));
            foreach (var lItem in email.Receiver)
            {
                lMessage.To.Add(new MailboxAddress(lItem));
            }
            lMessage.Subject = email.Subject;
            var lBodyBuilder = new BodyBuilder() { HtmlBody = email.Body };
            lMessage.Body = lBodyBuilder.ToMessageBody();
            return Send(lMessage);
        }

        private ResultMsg Send(MimeMessage message)
        {
            var lSmtpClient = new SmtpClient { Timeout = 10 * 1000 };
            try
            {
                var lHost = mConfigService.SmtpHost;
                var lPort = mConfigService.SmtpPort;
                var lAddress = mConfigService.Address;
                var lPassword = mConfigService.SecurityCode;
                lSmtpClient.Connect(lHost, lPort, MailKit.Security.SecureSocketOptions.Auto); //连接到远程smtp服务器
                lSmtpClient.Authenticate(lAddress, lPassword);
                lSmtpClient.Send(message); //发送邮件
                lSmtpClient.Disconnect(true);
                //mLogService.Info(this, $"邮件发送成功,接收方：{message.To.Join(",")}");
                return CreateResultMsg("邮件发送成功");
            }
            catch (Exception lEx)
            {
                mLogService.Error(this, $"邮件发送失败,接收方：{message.To.Join(",")}", lEx);
                return CreateErrorMsg($"邮件发送失败：{lEx.Message}");
            }
            finally
            {
                lSmtpClient.Dispose();
            }
        }

        protected static ResultMsg CreateResultMsg(string message = null)
        {
            var lMsg = new ResultMsg
            {
                Status = 0,
                Message = message
            };
            return lMsg;
        }

        protected static ResultMsg CreateErrorMsg(string message = null)
        {
            var lMsg = new ResultMsg
            {
                Status = 1,
                Message = message
            };
            return lMsg;
        }
    }
}
