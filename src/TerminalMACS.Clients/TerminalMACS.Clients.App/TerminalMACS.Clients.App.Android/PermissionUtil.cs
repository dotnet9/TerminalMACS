using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TerminalMACS.Clients.App.Droid
{
    public static class PermissionUtil
    {
        /**
		* 通过验证给定数组中的每个条目的值是否为Permission.Granted，检查是否已授予所有给定权限。
		*
		* See Activity#onRequestPermissionsResult (int, String[], int[])
		*/
        public static bool VerifyPermissions(Permission[] grantResults)
        {
            // 必须至少检查一个结果.
            if (grantResults.Length < 1)
                return false;

            // 验证是否已授予每个必需的权限，否则返回false.
            foreach (Permission result in grantResults)
            {
                if (result != Permission.Granted)
                {
                    return false;
                }
            }
            return true;
        }
    }
}