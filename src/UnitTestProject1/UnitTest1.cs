﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlSugar;
using System;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {


            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = "Data Source=101.34.154.180,51433;Initial Catalog=LabelTest;uid=sa;pwd=Pass@Word;",//连接符字串
                DbType = DbType.SqlServer,// DbType.SqlServer,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute,//从特性读取主键自增信息
            });

            //DbFirst测试
            db.CodeFirst.SplitTables().InitTables<ScheduleEntity>();

            //db.CodeFirst.sp
        }
    }
}
