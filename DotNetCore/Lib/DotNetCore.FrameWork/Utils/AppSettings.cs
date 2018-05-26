 

/*****************************************************************************
 * 
 * Created On: 2018-05-22
 * Purpose:    获取配置文件
 * 
 ****************************************************************************/

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace DotNetCore.FrameWork.Utils
{
    public class AppSettings
    {
        public static IConfiguration Configuration { get; set; }

        static AppSettings()
        {
            /*
              ReloadOnChange = true 当appsettings.json被修改时重新加载
              AppSettings.Configuration["first"];  读取一级目录
              AppSettings.Configuration["first:XXX"];   读取二级目录
              AppConfigurtaionServices.Configuration.GetConnectionString("key"); 读取链接串
            */

            Configuration = new ConfigurationBuilder()
                            .Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true })
                            .Build();
        }
    }
}
