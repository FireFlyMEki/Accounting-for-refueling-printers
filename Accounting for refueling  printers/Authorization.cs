using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Accounting_for_refueling__printers
{
    public partial class Authorization : Form
    {
        private SqlConnection sqlConnection;
        
        
        public Authorization()
        {
            Thread t = new Thread(new ThreadStart(StartForm));
            t.Start();
            Thread.Sleep(5000);
            TopMost = true;
            InitializeComponent();
            Console.WriteLine(Hashing.Hash("admin"));
            t.Abort(t);
        }

            
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void Authorization_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename =" + Application.StartupPath + @"\Database.mdf; Integrated Security = True");
            sqlConnection.Open();
        }
        public void StartForm()
        {
            Application.Run(new SplashScreen());
        }
      


        void InputOnSystem(string Value)
        {
            this.TopMost = false;
            this.Hide();
            AcessRight.Acess = int.Parse(Value);
            FormMainMenu formMainMenu = new FormMainMenu();
            formMainMenu.Show();
        }

        private void btnInput_Click(object sender, EventArgs e)
        {
            if (textLogin.Text != null || textPassword != null)
            {
                var LoginUser = textLogin.Text;
                var PasswordUser = textPassword.Text;
                //1

                //2
               


                if (AuthorizeUser(textLogin.Text, textPassword.Text))
                {
                     SqlCommand RightAcess = new SqlCommand(@"Select RightAcess from Account where LoginUser= @ul and PasswordUser= @up ", sqlConnection);
                RightAcess.Parameters.Add("@ul", sqlDbType: SqlDbType.VarChar).Value = LoginUser;
                RightAcess.Parameters.Add("@up", sqlDbType: SqlDbType.VarChar).Value = Hashing.Hash(PasswordUser);
                        InputOnSystem(RightAcess.ExecuteScalar().ToString());


                }
                else
                {
                    MessageBox.Show("Заполните все поля", "Ошибка");
                }

            }

           

          

        }

        private void panelHeader_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
             Application.Exit();
        }
        public bool AuthorizeUser(string login, string password)
        {
           
                bool isAuthorized = false;
          
               
 
                    using (SqlCommand command = new SqlCommand($"Select Account_ID, LoginUser, PasswordUser from Account where [LoginUser] = '{login}' and [PasswordUser] = '{Hashing.Hash(password)}'", sqlConnection))
                    {
                        if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                        {
                            isAuthorized = true;
                            
                            MessageBox.Show("Вход выполнен", "Успех");
                        }
                        else
                        {
                            MessageBox.Show("Логин или пароль неверный", "Ошибка входа");
                        }
                    }
                
                return isAuthorized;
         

        }

        private void panelAutorization_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
