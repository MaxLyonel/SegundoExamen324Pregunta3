using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;


namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public int id = 0, cR, cB, cG;
        public Boolean sw = true;
        public Bitmap bmp = null, copia=null, cop = null;
        public String color = String.Empty;
        SqlConnection con;
        public string conString = "Data Source=DESKTOP-U1FMLGK\\SQLEXPRESS;Initial Catalog=SegundoExamen324;Integrated Security=True; MultipleActiveResultSets=true";
        public Form1()
        {
            InitializeComponent();
            conexion();
        }

        public void conexion()
        {
            con = new SqlConnection(); // Un objeto que nos ayuda a hacer la conexión
            //SqlDataAdapter ada = new SqlDataAdapter();
            //DataSet ds = new DataSet();
            con.ConnectionString = conString; // acá se manda la cadena de conexión
        
            con.Open();
            if (con.State == System.Data.ConnectionState.Open)
                Console.WriteLine("Conexión con SQL-Server exitosa!");
            else
                Console.WriteLine("No se pudo conectar a la BD");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            Bitmap bmp = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = bmp;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {           
            Bitmap bmp = new Bitmap(pictureBox1.Image);
            Color c = new Color();
            int x, y, mR = 0, mG = 0, mB = 0;
            x = e.X;
            y = e.Y;
            for (int i = x; i < x + 10; i++)
            {
                for (int j = y; j < y + 10; j++)
                {
                    c = bmp.GetPixel(i, j);
                    mR = mR + c.R;
                    mG = mG + c.G;
                    mB = mB + c.B;
                }
            }
            mR = mR / 100;
            mG = mG / 100;
            mB = mB / 100;
            cR = mR;
            cG = mG;
            cB = mB;
            
            textBox1.Text = cR.ToString();
            textBox2.Text = cG.ToString();
            textBox3.Text = cB.ToString();
            
        }
        // Cambio click 2
        private void button7_Click(object sender, EventArgs e)
        {                        
            int meR, meG, meB;
            if (sw)
            {
                Console.WriteLine("primera vez");
                bmp = new Bitmap(pictureBox1.Image);
                copia = new Bitmap(bmp.Width, bmp.Height);
                sw = false;
            }
            Color c = new Color();
            // Acá se realiza la consulta
            SqlCommand sel = new SqlCommand();
            sel.Connection = con;
            sel.CommandText = @"
                SELECT color, R,G,B FROM TEXTURAS  
            ";
            try
            {
                SqlDataReader dr = sel.ExecuteReader();
                // Esto va iterar la cantidade de datos recuperados
                while (dr.Read())
                {
                    // Por cada iteración se va a obtener su color y sus color de imagen
                    string col = dr["color"].ToString();
                    int aR = (int)dr["R"];
                    int aG = (int)dr["G"];
                    int aB = (int)dr["B"];
                    
                    // Acá se recorre toda la imagen
                    for (int i = 0; i < bmp.Width - 10; i += 10)
                    {
                        for (int j = 0; j < bmp.Height - 10; j += 10)
                        {
                            meR = 0;
                            meG = 0;
                            meB = 0;
                            for (int k = i; k < i + 10; k++)
                                for (int l = j; l < j + 10; l++)
                                {
                                    c = bmp.GetPixel(k, l);
                                    meR = meR + c.R;
                                    meG = meG + c.G;
                                    meB = meB + c.B;
                                }
                            meR = meR / 100;
                            meG = meG / 100;
                            meB = meB / 100;
                            
                            if ((aR - 10 < meR) && (meR < aR + 10) && (aG - 10 < meG) && (meG < aG + 10) && (aB - 10 < meB) && (meB < aB + 10))
                            {
                                // Acá se va pintando los píxeles 
                                for (int k = i; k < i + 10; k++)
                                    for (int l = j; l < j + 10; l++)
                                    {
                                        switch (color)
                                        {
                                            case "ROJO":
                                                copia.SetPixel(k, l, Color.Red); 
                                                break;
                                            case "AZUL":
                                                copia.SetPixel(k, l, Color.Blue);
                                                break;
                                            case "VERDE":
                                                copia.SetPixel(k, l, Color.Green);
                                                break;
                                            case "AMARILLO":
                                                copia.SetPixel(k, l, Color.Yellow);
                                                break;
                                            case "NARANJA":
                                                copia.SetPixel(k, l, Color.Orange);
                                                break;
                                            case "ROSADO":
                                                copia.SetPixel(k, l, Color.Pink);
                                                break;
                                        }                                        
                                    }                        
                            }
                            else
                            {
                                for (int k = i; k < i + 10; k++)
                                    for (int l = j; l < j + 10; l++)
                                    {
                                        c = bmp.GetPixel(k, l);
                                        
                                        copia.SetPixel(k, l, c);                                
                                    }                        
                            }
                        }
                    }
                    bmp = copia; // Esto soluciono parte del problema                                        
                }
                pictureBox2.Image = copia;                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int indice = comboBox1.SelectedIndex;
            color = comboBox1.Items[indice].ToString();
            //Console.WriteLine(color);
        }

        private void Almacenar_Click(object sender, EventArgs e)
        {
            // aca se tiene que ejecutar la instrucción de insert
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            // Enviando parámetros a la consulta
            cmd.Parameters.Add("@ID", SqlDbType.Int);
            cmd.Parameters["@ID"].Value = id;
            cmd.Parameters.AddWithValue("@color", color); // hay que determinar el color de inserción en la base de datos
            cmd.Parameters.AddWithValue("@R", cR);
            cmd.Parameters.AddWithValue("@G", cG);
            cmd.Parameters.AddWithValue("@B", cB);
            cmd.CommandText =
                "Insert Into TEXTURAS (codigo, color, R,G,B)" +
                " Values (@ID, @color, @R, @G, @B)";                       
            try
            {               
                Int32 rowsAffected = cmd.ExecuteNonQuery();
                Console.WriteLine("RowsAffected: {0}", rowsAffected);
                id++;
                textBox1.Text = String.Empty;
                textBox2.Text = String.Empty;
                textBox3.Text = String.Empty;
                
            }
            catch (Exception ex)
            {               
                Console.WriteLine("Quizá, no llenaste datos");
                Console.WriteLine(ex.Message);
            }
        }

        private void Mostrar_Texturas(object sender, EventArgs e)
        {
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
        }
    }
}
