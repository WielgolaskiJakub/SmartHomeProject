using System.Data.SQLite;

namespace SmartHomeProject
{
    public partial class MainGUI : Form
    {
        private const string GreenCirclePath = @"C:\Users\Jakub\Desktop\Zielone koło Emoji 🟢_files\large-green-circle_1f7e2.png";
        private const string RedCirclePath = @"C:\Users\Jakub\Desktop\Czerwone koło Emoji 🔴_files\large-red-circle_1f534(4).png";
        private const string HomePicturePath = @"C:\Users\Jakub\Desktop\Mieszkanie.jpg";

        private Image greenCircleImage;
        private Image redCircleImage;
        private Image homePictureImage;
        private JCS.ToggleSwitch gateSwitch;
        private JCS.ToggleSwitch lightsSwitch;
        private JCS.ToggleSwitch windowsBlindsSwitch;
        private SmartHomeTimer smartHomeTimer;
        private System.Windows.Forms.Timer guiUpdateTimer;
        private Lights lights;
        private WindowBlinds windowBlinds;
        private bool pictureBox4State;
        private bool pictureBox5State;
        private bool pictureBox6State;

        public MainGUI()
        {
            InitializeComponent();


            smartHomeTimer = new SmartHomeTimer();
            lights = new Lights(smartHomeTimer);
            windowBlinds = new WindowBlinds(smartHomeTimer);
            //tło
            this.BackColor = Color.LightCyan;


            string basePath = Application.StartupPath;
            greenCircleImage = LoadImage(Path.Combine(basePath, GreenCirclePath));
            redCircleImage = LoadImage(Path.Combine(basePath, RedCirclePath));
            homePictureImage = LoadImage(Path.Combine(basePath, HomePicturePath));

            ConfigurePictureBox(pictureBox1, redCircleImage, new Size(40, 40));
            ConfigurePictureBox(pictureBox2, redCircleImage, new Size(40, 40));
            ConfigurePictureBox(pictureBox3, homePictureImage, new Size(430, 350));
            ConfigurePictureBox(pictureBox4, redCircleImage, new Size(20, 20));
            pictureBox4.Click += PictureBox4_Click;
            ConfigurePictureBox(pictureBox5, redCircleImage, new Size(20, 20));
            pictureBox5.Click += PictureBox5_Click;
            ConfigurePictureBox(pictureBox6, redCircleImage, new Size(20, 20));
            pictureBox6.Click += PictureBox6_Click;

            ConfigureTrackBar(trackBar1, 0, 23, 12, 1, LightsTimeStart);
            ConfigureTrackBar(trackBar2, 0, 23, 12, 1, LightsTimeEnd);
            ConfigureTrackBar(trackBar3, 0, 23, 12, 1, WindowBlindsStart);
            ConfigureTrackBar(trackBar4, 0, 23, 12, 1, WindowBlindsEnd);


            ConfigureCheckBox(checkBox1, "Automatyczne Sterowanie Oświetleniem Zewnętrznym", autoLightsCheckbox_Click);
            ConfigureCheckBox(checkBox2, "Automatyczne Sterowanie Roletami Okiennymi", windowsBlindsCheckBox_Click);


            UpdateLabelText(label2, trackBar1.Value, "Start");
            UpdateLabelText(label3, trackBar2.Value, "Zakończ");
            UpdateLabelText(label4, trackBar3.Value, "Start");
            UpdateLabelText(label5, trackBar4.Value, "Zakończ");

            label1.Text = "Temperatura";
            label6.Text = "Światło";
            label7.Text = "Brama";
            label10.Text = "Nieaktywne";
            label11.Text = "Nieaktywne";
            label13.Text = "Rolety";
            //zegar 
            label12.Text = "Zegar: 00 h";
            label12.Font = new Font("Arial", 14, FontStyle.Bold);
            label12.AutoSize = true;
            label8.Text = "Światło I";
            label9.Text = "Światło II";
            label14.Text = "Światło III";
            //uruchom
            button1.Text = "Uruchom";
            button1.Click += startButton_Click;

            //zatrzymaj
            button2.Text = "Stop";
            button2.Click += stopButton_Click;

            button3.Text = "Zapisz";
            button3.Click += saveButton_Click;

            //ukrywanie trackabara
            trackBar1.Enabled = false;
            trackBar2.Enabled = false;
            trackBar3.Enabled = false;
            trackBar4.Enabled = false;

            lightsSwitch = new JCS.ToggleSwitch();
            ConfigureToggleSwitch(lightsSwitch, 547, 47, 110, 30, "ON", "OFF", lightsSwitch_CheckedChanged);

            gateSwitch = new JCS.ToggleSwitch();
            ConfigureToggleSwitch(gateSwitch, 547, 92, 110, 30, "OPEN", "CLOSED", gateSwitch_CheckedChanged);

            windowsBlindsSwitch = new JCS.ToggleSwitch();
            ConfigureToggleSwitch(windowsBlindsSwitch, 547, 137, 110, 30, "OPEN", "CLOSED", windowsBlindsSwitch_CheckedChanged);

            //guiTimer
            guiUpdateTimer = new System.Windows.Forms.Timer();
            guiUpdateTimer.Interval = 1000;
            guiUpdateTimer.Tick += GuiUpdateTimer_Tick;



            this.Controls.Add(pictureBox1);
            this.Controls.Add(pictureBox2);
            this.Controls.Add(pictureBox3);
            this.Controls.Add(pictureBox4);
            this.Controls.Add(pictureBox5);
            this.Controls.Add(pictureBox6);
            this.Controls.Add(lightsSwitch);
            this.Controls.Add(gateSwitch);
            this.Controls.Add(windowsBlindsSwitch);

            pictureBox4.BringToFront();
            pictureBox5.BringToFront();
            pictureBox6.BringToFront();

            LoadValuesFromDatabase();
        }

        private void PictureBox4_Click(object? sender, EventArgs e)
        {
            // Zmień stan lokalny
            pictureBox4State = !pictureBox4State;

            // Zmień obraz w PictureBox
            pictureBox4.Image = pictureBox4State ? greenCircleImage : redCircleImage;
        }

        private void PictureBox5_Click(object? sender, EventArgs e)
        {
            pictureBox5State = !pictureBox5State;
            pictureBox5.Image = pictureBox5State ? greenCircleImage : redCircleImage;
        }

        private void PictureBox6_Click(object? sender, EventArgs e)
        {
            pictureBox6State = !pictureBox6State;
            pictureBox6.Image = pictureBox6State ? greenCircleImage : redCircleImage;
        }


        private void checkLightsStartAndEnd()
        {
            bool isEnabled = lights.IsLightsEnabled();

            if (isEnabled)
            {
                label10.Text = "Aktywne";
                pictureBox1.Image = greenCircleImage;
            }
            else
            {
                label10.Text = "Nieaktywne";
                pictureBox1.Image = redCircleImage;
            }

            label10.Refresh();
            pictureBox1.Refresh();
        }
        private void checkWindowBlindsStartAndEnd()
        {
            bool isEnabled = windowBlinds.IsWindowBlindsEnabled();

            if (isEnabled)
            {
                label11.Text = "Aktywne";
                pictureBox2.Image = greenCircleImage;
            }
            else
            {
                label11.Text = "Nieaktywne";
                pictureBox2.Image = redCircleImage;
            }

            label11.Refresh();
            pictureBox2.Refresh();
        }
        private void stopButton_Click(object? sender, EventArgs e)
        {
            smartHomeTimer.Stop();
            guiUpdateTimer.Stop();
            label12.Text = "Zegar: 00h";
        }

        private void GuiUpdateTimer_Tick(object? sender, EventArgs e)
        {
            label12.Text = "Zegar: " + smartHomeTimer.SimulatedTime.ToString("HH") + " h";
            if (checkBox1.Checked)
            {
                checkLightsStartAndEnd();
            }
            else
            {
                pictureBox1.Image = redCircleImage;
                pictureBox1.Refresh();
            }
            if (checkBox2.Checked)
            {
                checkWindowBlindsStartAndEnd();
            }
            else
            {
                pictureBox2.Image = redCircleImage;
                pictureBox2.Refresh();
            }
        }

        private void ConfigurePictureBox(PictureBox pictureBox, Image image, Size size)
        {
            pictureBox.Size = size;
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.Image = image;
        }


        private Image LoadImage(string path)
        {
            try
            {
                return Image.FromFile(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd ładowania obrazu: {ex.Message}");
                return null;
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            LoadValuesFromDatabase();
            smartHomeTimer.Start();
            guiUpdateTimer.Start();


        }

        private void saveButton_Click(object sender, EventArgs e)
        {

            if (!int.TryParse(textBox1.Text, out int temperature))
            {
                MessageBox.Show("Pole Temperatura nie może być puste!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int lightsStart = trackBar1.Value;
            int lightsStop = trackBar2.Value;
            int blindsStart = trackBar3.Value;
            int blindsStop = trackBar4.Value;
            bool isLightOn = lightsSwitch.Checked;
            bool isGateOpen = gateSwitch.Checked;
            bool isBlindsOpen = windowsBlindsSwitch.Checked;
            bool isWindowsBlindEnable = checkBox2.Checked;
            bool isLightsEnable = checkBox1.Checked;
            bool pictureBox4State = pictureBox4.Image == greenCircleImage;
            bool pictureBox5State = pictureBox5.Image == greenCircleImage;
            bool pictureBox6State = pictureBox6.Image == greenCircleImage;

            DataBaseInitializer.UpdateColumn("PictureBox4State", pictureBox4State ? 1 : 0);
            DataBaseInitializer.UpdateColumn("PictureBox5State", pictureBox5State ? 1 : 0);
            DataBaseInitializer.UpdateColumn("PictureBox6State", pictureBox6State ? 1 : 0);
            DataBaseInitializer.UpdateColumn("Temperature", temperature);
            DataBaseInitializer.UpdateColumn("Lights", isLightOn ? 1 : 0);
            DataBaseInitializer.UpdateColumn("WindowBlinds", isBlindsOpen ? 1 : 0);
            DataBaseInitializer.UpdateColumn("AutoLightsStart", lightsStart);
            DataBaseInitializer.UpdateColumn("AutoLightsEnd", lightsStop);
            DataBaseInitializer.UpdateColumn("AutoBlindsStart", blindsStart);
            DataBaseInitializer.UpdateColumn("AutoBlindsEnd", blindsStop);
            DataBaseInitializer.UpdateColumn("Gate", isGateOpen ? 1 : 0);
            DataBaseInitializer.UpdateColumn("LightsCheckBox", isLightsEnable ? 1 : 0);
            DataBaseInitializer.UpdateColumn("WindowBlindsCheckBox", isWindowsBlindEnable ? 1 : 0);

            LoadValuesFromDatabase();

            this.Refresh();
        }

        private void ConfigureToggleSwitch(
     JCS.ToggleSwitch toggleSwitch,
     int x,
     int y,
     int width,
     int height,
     string textOn,
     string textOff,
     JCS.ToggleSwitch.CheckedChangedDelegate checkedChangedEvent)
        {

            toggleSwitch.Location = new Point(x, y);
            toggleSwitch.Size = new Size(width, height);
            toggleSwitch.OnText = textOn;
            toggleSwitch.OffText = textOff;
            toggleSwitch.Checked = false;
            toggleSwitch.Style = JCS.ToggleSwitch.ToggleSwitchStyle.Fancy;
            if (checkedChangedEvent != null)
            {
                toggleSwitch.CheckedChanged += checkedChangedEvent;
            }
        }


        public delegate void CheckedChangedDelegate(object sender, EventArgs e);

        private void ConfigureTrackBar(TrackBar trackBar, int min, int max, int value, int tickFrequency, EventHandler valueChangedEvent)
        {
            trackBar.Minimum = min;
            trackBar.Maximum = max;
            trackBar.Value = value;
            trackBar.TickFrequency = tickFrequency;
            trackBar.ValueChanged += valueChangedEvent;
        }



        private void UpdateLabelText(Label label, int value, string prefix)
        {
            label.Text = $"{prefix}: {value}:00";
        }


        private void ConfigureCheckBox(CheckBox checkBox, string text, EventHandler checkedChangedEvent)
        {
            checkBox.Text = text;
            checkBox.CheckedChanged += checkedChangedEvent;
        }

        private void autoLightsCheckbox_Click(object sender, EventArgs e)
        {

            bool isChecked = checkBox1.Checked;

            if (isChecked)
            {
                trackBar1.Enabled = true;
                trackBar2.Enabled = true;
            }
            else
            {
                trackBar1.Enabled = false;
                trackBar2.Enabled = false;
            }
        }
        private void windowsBlindsCheckBox_Click(object sender, EventArgs e)
        {
            bool isChecked = checkBox2.Checked;

            if (isChecked)
            {
                trackBar3.Enabled = true;
                trackBar4.Enabled = true;
            }
            else
            {
                trackBar3.Enabled = false;
                trackBar4.Enabled = false;
            }
        }


        private void LightsTimeStart(object sender, EventArgs e)
        {
            UpdateLabelText(label2, trackBar1.Value, "Start");
        }

        private void LightsTimeEnd(object sender, EventArgs e)
        {
            UpdateLabelText(label3, trackBar2.Value, "Zakończ");
        }

        private void WindowBlindsStart(object sender, EventArgs e)
        {
            UpdateLabelText(label4, trackBar3.Value, "Start");
        }

        private void WindowBlindsEnd(object sender, EventArgs e)
        {
            UpdateLabelText(label5, trackBar4.Value, "Zakończ");
        }

        private void lightsSwitch_CheckedChanged(object sender, EventArgs e)
        {
            var toggleSwitch = sender as JCS.ToggleSwitch;
            if (toggleSwitch != null)
            {
                bool isChecked = toggleSwitch.Checked;
            }
        }

        private void gateSwitch_CheckedChanged(object sender, EventArgs e)
        {
            var toggleSwitch = sender as JCS.ToggleSwitch;
            if (toggleSwitch != null)
            {
                bool isChecked = toggleSwitch.Checked;
            }
        }

        private void windowsBlindsSwitch_CheckedChanged(Object sender, EventArgs e)
        {
            var toggleSwitch = sender as JCS.ToggleSwitch;
            if (toggleSwitch != null)
            {
                bool isChecked = toggleSwitch.Checked;
            }
        }

        private void LoadValuesFromDatabase()
        {
            int temperature = Convert.ToInt32(DataBaseInitializer.GetColumnValue("Temperature"));
            int lightsStart = Convert.ToInt32(DataBaseInitializer.GetColumnValue("AutoLightsStart"));
            int lightsEnd = Convert.ToInt32(DataBaseInitializer.GetColumnValue("AutoLightsEnd"));
            int blindsStart = Convert.ToInt32(DataBaseInitializer.GetColumnValue("AutoBlindsStart"));
            int blindsEnd = Convert.ToInt32(DataBaseInitializer.GetColumnValue("AutoBlindsEnd"));
            bool isLightOn = Convert.ToInt32(DataBaseInitializer.GetColumnValue("Lights")) == 1;
            bool isGateOpen = Convert.ToInt32(DataBaseInitializer.GetColumnValue("Gate")) == 1;
            bool isBlindsOpen = Convert.ToInt32(DataBaseInitializer.GetColumnValue("WindowBlinds")) == 1;
            bool islightsEnabled = Convert.ToInt32(DataBaseInitializer.GetColumnValue("LightsCheckBox")) == 1;
            bool isWindowBlindsEnbaled = Convert.ToInt32(DataBaseInitializer.GetColumnValue("WindowBlindsCheckBox")) == 1;
            bool pictureBox4State = Convert.ToInt32(DataBaseInitializer.GetColumnValue("PictureBox4State")) == 1;
            bool pictureBox5State = Convert.ToInt32(DataBaseInitializer.GetColumnValue("PictureBox5State")) == 1;
            bool pictureBox6State = Convert.ToInt32(DataBaseInitializer.GetColumnValue("PictureBox6State")) == 1;



            textBox1.Text = temperature.ToString();
            trackBar1.Value = lightsStart;
            trackBar2.Value = lightsEnd;
            trackBar3.Value = blindsStart;
            trackBar4.Value = blindsEnd;
            lightsSwitch.Checked = isLightOn;
            gateSwitch.Checked = isGateOpen;
            windowsBlindsSwitch.Checked = isBlindsOpen;
            checkBox1.Checked = islightsEnabled;
            checkBox2.Checked = isWindowBlindsEnbaled;
            pictureBox4.Image = pictureBox4State ? greenCircleImage : redCircleImage;
            pictureBox5.Image = pictureBox5State ? greenCircleImage : redCircleImage;
            pictureBox6.Image = pictureBox6State ? greenCircleImage : redCircleImage;
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
           
        }




    }

}


// Obrazek domu w tle - 3 pokoje, przycisk od świateł w każdym pokoju, który symuluje działanie poprzez przełacznik i zmianę koloru
// grzałka temperatura ze światełkiem, symulowanie temperatury 
// w jednym folderze plik exe, sprawozdanie oraz prezentacja.