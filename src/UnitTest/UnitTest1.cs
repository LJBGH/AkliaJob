using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlSugar;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

            
            SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = "Data Source=101.34.154.180,51433;Initial Catalog=LabelTest;uid=sa;pwd=Pass@Word;",//���ӷ��ִ�
                DbType = DbType.SqlServer,// DbType.SqlServer,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute,//�����Զ�ȡ����������Ϣ
            });

            //DbFirst����
            db.CodeFirst.InitTables(typeof(/*LabelEntity*/ string));
        }
    }
}
