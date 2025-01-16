using SmartHomeProject;

Application.EnableVisualStyles();
Application.SetCompatibleTextRenderingDefault(false);
Application.Run(new MainGUI());


SmartHomeTimer timer = new SmartHomeTimer();
timer.Start();