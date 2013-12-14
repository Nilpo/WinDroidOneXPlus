using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using MetroFramework.Forms;
using Microsoft.VisualBasic;
using RegawMOD.Android;

namespace WinDroid_HTC_One_X__Toolkit
{
    public partial class MainForm : MetroForm
    {
        private AndroidController _android;
        private Device _device;

        public static class AndroidLib
        {
            public static string InitialCMD = "";
            public static string SecondaryCMD = "";
            public static string Selector = "";
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (Directory.Exists("C:/Program Files (x86)/ClockworkMod/Universal Adb Driver"))
                {
                }
                else
                {
                    if (Directory.Exists("C:/Program Files/ClockworkMod/Universal Adb Driver"))
                    {
                    }
                    else
                    {
                        using (var sr = new StreamReader("./Data/Settings/ADB.ini"))
                        {
                            var line = sr.ReadToEnd();
                            if (line == "Yes")
                            {
                                var dialogResult2 =
                                    MessageBox.Show(
                                        @"You are missing some ADB Drivers! They are required for your phone to connect properly with the computer. Would you like to download and install them now? The download may take up to 5 minutes.",
                                        @"ADB Drivers", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                if (dialogResult2 == DialogResult.Yes)
                                {
                                    downloadADB.RunWorkerAsync();
                                }
                                else if (dialogResult2 == DialogResult.No)
                                {
                                    var dialogResult3 =
                                        MessageBox.Show(
                                            @"Would you like to be reminded of this the next time you open the toolkit?",
                                            @"ADB Drivers", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                    if (dialogResult3 == DialogResult.Yes)
                                    {
                                        sr.Close();
                                        File.WriteAllText("./Data/Settings/ADB.ini", @"Yes");
                                    }
                                    else if (dialogResult3 == DialogResult.No)
                                    {
                                        sr.Close();
                                        File.WriteAllText("./Data/Settings/ADB.ini", @"No");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }

            try
            {
                deviceRecognition.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }

            try
            {
                downloadFiles.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void deviceRecognition_DoWork(object sender, DoWorkEventArgs e)
        {
            _android = AndroidController.Instance;
            try
            {
                _android.UpdateDeviceList();
                if (_android.HasConnectedDevices)
                {
                    _device = _android.GetConnectedDevice(_android.ConnectedDevices[0]);
                    if (_device.State.ToString() == "ONLINE")
                    {
                        if (_device.BuildProp.GetProp("ro.product.model") == null)
                        {
                            deviceLabelChange.Text = _device.SerialNumber;
                            statusLabelChange.Text = @"Online";
                            statusLabel.Location = new Point(deviceLabelChange.Location.X + deviceLabelChange.Width,
                                deviceLabelChange.Location.Y);
                            statusLabelChange.Location = new Point(statusLabel.Location.X + statusLabel.Width - 5,
                                statusLabel.Location.Y);
                            statusProgressSpinner.Location = new Point(statusLabel.Location.X + statusLabel.Width,
                                statusLabel.Location.Y);
                            statusProgressSpinner.Visible = false;
                        }
                        else
                        {
                            deviceLabelChange.Text = _device.BuildProp.GetProp("ro.product.model");
                            statusLabelChange.Text = @"Online";
                            statusLabel.Location = new Point(deviceLabelChange.Location.X + deviceLabelChange.Width,
                                deviceLabelChange.Location.Y);
                            statusLabelChange.Location = new Point(statusLabel.Location.X + statusLabel.Width - 5,
                                statusLabel.Location.Y);
                            statusProgressSpinner.Location = new Point(statusLabel.Location.X + statusLabel.Width,
                                statusLabel.Location.Y);
                            statusProgressSpinner.Visible = false;
                        }
                    }
                    else if (_device.State.ToString() == "FASTBOOT")
                    {
                        deviceLabelChange.Text = _device.SerialNumber;
                        statusLabelChange.Text = @"Fastboot";
                        statusLabel.Location = new Point(deviceLabelChange.Location.X + deviceLabelChange.Width,
                            deviceLabelChange.Location.Y);
                        statusLabelChange.Location = new Point(statusLabel.Location.X + statusLabel.Width - 5,
                            statusLabel.Location.Y);
                        statusProgressSpinner.Location = new Point(statusLabel.Location.X + statusLabel.Width,
                            statusLabel.Location.Y);
                        statusProgressSpinner.Visible = false;
                    }
                    else if (_device.State.ToString() == "RECOVERY")
                    {
                        deviceLabelChange.Text = _device.SerialNumber;
                        statusLabelChange.Text = @"Recovery";
                        statusLabel.Location = new Point(deviceLabelChange.Location.X + deviceLabelChange.Width,
                            deviceLabelChange.Location.Y);
                        statusLabelChange.Location = new Point(statusLabel.Location.X + statusLabel.Width - 5,
                            statusLabel.Location.Y);
                        statusProgressSpinner.Location = new Point(statusLabel.Location.X + statusLabel.Width,
                            statusLabel.Location.Y);
                        statusProgressSpinner.Visible = false;
                    }
                    else if (_device.State.ToString() == "UNKNOWN")
                    {
                        deviceLabelChange.Text = _device.SerialNumber;
                        statusLabelChange.Text = @"Unknown";
                        statusLabel.Location = new Point(deviceLabelChange.Location.X + deviceLabelChange.Width,
                            deviceLabelChange.Location.Y);
                        statusLabelChange.Location = new Point(statusLabel.Location.X + statusLabel.Width - 5,
                            statusLabel.Location.Y);
                        statusProgressSpinner.Location = new Point(statusLabel.Location.X + statusLabel.Width,
                            statusLabel.Location.Y);
                        statusProgressSpinner.Visible = false;
                    }
                    deviceProgressSpinner.Visible = false;
                    refreshSpinner.Visible = false;
                }
                else
                {
                    deviceLabelChange.Text = @"Not Found!";
                    statusLabelChange.Text = @"Not Found!";
                    statusLabel.Location = new Point(deviceLabelChange.Location.X + deviceLabelChange.Width,
                        deviceLabelChange.Location.Y);
                    statusLabelChange.Location = new Point(statusLabel.Location.X + statusLabel.Width - 5,
                        statusLabel.Location.Y);
                    deviceProgressSpinner.Visible = false;
                    statusProgressSpinner.Visible = false;
                    refreshSpinner.Visible = false;
                }
                _android.Dispose();
                refreshButton.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void downloadADB_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var client = new WebClient();
                client.DownloadFile("http://download.clockworkmod.com/test/UniversalAdbDriverSetup.msi",
                    "./Data/Installers/ADB.msi");
                Process.Start(Application.StartupPath + "/Data/Installers/ADB.msi");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void downloadFiles_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (File.Exists("./Data/Installers/Superuser.zip"))
                {
                }
                else
                {
                    var client = new WebClient();
                    client.DownloadFile("http://download.clockworkmod.com/superuser/superuser.zip",
                        "./Data/Installers/Superuser.zip");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
            try
            {
                if (File.Exists("./Data/Installers/Xposed.apk"))
                {
                }
                else
                {
                    var client = new WebClient();
                    client.DownloadFile("http://dl.xposed.info/latest.apk", "./Data/Installers/Xposed.apk");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
            try
            {
                if (File.Exists("./Data/Installers/Busybox.apk"))
                {
                }
                else
                {
                    var client = new WebClient();
                    client.DownloadFile("http://fs1.d-h.st/download/00050/A2j/stericson.busybox-141-v9.3.apk",
                        "./Data/Installers/Busybox.apk");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void noReturnADBCommand_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Adb.ExecuteAdbCommandNoReturn(Adb.FormAdbCommand(AndroidLib.InitialCMD));
                _android.Dispose();
                loadingSpinner.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void noReturnADBCommand_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (AndroidLib.Selector == "tokenID")
                {
                    loadingSpinner.Visible = true;
                    AndroidLib.InitialCMD = "oem";
                    AndroidLib.SecondaryCMD = "get_identifier_token";
                    tokenID.RunWorkerAsync();
                }
                else if (AndroidLib.Selector == "bootloaderUnlock")
                {
                    loadingSpinner.Visible = true;
                    AndroidLib.InitialCMD = "flash";
                    AndroidLib.SecondaryCMD = "unlocktoken " + openFileDialog1.FileName;
                    noReturnFastbootCommand.RunWorkerAsync();
                }
                else if (AndroidLib.Selector == "internationalTWRP")
                {
                    loadingSpinner.Visible = true;
                    AndroidLib.InitialCMD = "flash";
                    AndroidLib.SecondaryCMD = "recovery ./Data/Recoveries/TWRPINT.img";
                    AndroidLib.Selector = "internationalTWRP";
                    noReturnFastbootCommand.RunWorkerAsync();
                }
                else if (AndroidLib.Selector == "internationalCWM")
                {
                    loadingSpinner.Visible = true;
                    AndroidLib.InitialCMD = "flash";
                    AndroidLib.SecondaryCMD = "recovery ./Data/Recoveries/TWRPCWM.img";
                    AndroidLib.Selector = "internationalCWM";
                    noReturnFastbootCommand.RunWorkerAsync();
                }
                else if (AndroidLib.Selector == "attTWRP")
                {
                    loadingSpinner.Visible = true;
                    AndroidLib.InitialCMD = "flash";
                    AndroidLib.SecondaryCMD = "recovery ./Data/Recoveries/TWRPATT.img";
                    AndroidLib.Selector = "attTWRP";
                    noReturnFastbootCommand.RunWorkerAsync();
                }
                else if (AndroidLib.Selector == "attCWM")
                {
                    loadingSpinner.Visible = true;
                    AndroidLib.InitialCMD = "flash";
                    AndroidLib.SecondaryCMD = "recovery ./Data/Recoveries/CWMATT.img";
                    AndroidLib.Selector = "attCWM";
                    noReturnFastbootCommand.RunWorkerAsync();
                }
                else if (AndroidLib.Selector == "customRecovery")
                {
                    loadingSpinner.Visible = true;
                    AndroidLib.InitialCMD = "flash";
                    AndroidLib.SecondaryCMD = "recovery " + openFileDialog1.FileName;
                    AndroidLib.Selector = "customRecovery";
                    noReturnFastbootCommand.RunWorkerAsync();
                }
                else if (AndroidLib.Selector == "flashKernel")
                {
                    loadingSpinner.Visible = true;
                    AndroidLib.InitialCMD = "flash";
                    AndroidLib.SecondaryCMD = "boot " + openFileDialog1.FileName;
                    AndroidLib.Selector = "flashKernel";
                    noReturnFastbootCommand.RunWorkerAsync();
                }
                else if (AndroidLib.Selector == "systemRemount")
                {
                    loadingSpinner.Visible = false;
                    systemRemountButton.Enabled = true;
                    MessageBox.Show(@"The system was successfully remounted!", @"System Remount Successful!",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (AndroidLib.Selector == "systemApp")
                {
                    loadingSpinner.Visible = true;
                    AndroidLib.InitialCMD = openFileDialog1.FileName;
                    AndroidLib.SecondaryCMD = "/system/app/";
                    AndroidLib.Selector = "systemApp";
                    pushFile.RunWorkerAsync();
                }
                else if (AndroidLib.Selector == "flashTempKernel")
                {
                    loadingSpinner.Visible = true;
                    AndroidLib.InitialCMD = "boot";
                    AndroidLib.SecondaryCMD = openFileDialog1.FileName;
                    AndroidLib.Selector = "flashTempKernel";
                    noReturnFastbootCommand.RunWorkerAsync();
                }
                else if (AndroidLib.Selector == "flashTempRecovery")
                {
                    loadingSpinner.Visible = true;
                    AndroidLib.InitialCMD = "boot";
                    AndroidLib.SecondaryCMD = openFileDialog1.FileName;
                    AndroidLib.Selector = "flashTempRecovery";
                    noReturnFastbootCommand.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void noReturnFastbootCommand_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Fastboot.ExecuteFastbootCommandNoReturn(Fastboot.FormFastbootCommand(_device, AndroidLib.InitialCMD,
                    AndroidLib.SecondaryCMD));
                _android.Dispose();
                loadingSpinner.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void noReturnFastbootCommand_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (AndroidLib.Selector == "bootloaderUnlock")
                {
                    MessageBox.Show(
                        @"Your bootloader has been successfully unlocked! You may now proceed to flash a Custom Recovery, flash a ROM and Kernel, as well as gain Permanent Root.",
                        "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (AndroidLib.Selector == "internationalTWRP")
                {
                    internationalTWRPButton.Enabled = true;
                    MessageBox.Show(@"TWRP 2.6.3.0 for the International HTC One X+ has been flashed!",
                        @"Hurray for TWRP!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (AndroidLib.Selector == "internationalCWM")
                {
                    internationalCWMButton.Enabled = true;
                    MessageBox.Show(@"CWM 6.0.3.5 for the International HTC One X+ has been flashed!",
                        @"Hurray for CWM!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (AndroidLib.Selector == "attTWRP")
                {
                    attTWRPButton.Enabled = true;
                    MessageBox.Show(@"TWRP 2.6.3.0 for the AT&T HTC One X+ has been flashed!", @"Hurray for TWRP!",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (AndroidLib.Selector == "attCWM")
                {
                    attCWMButton.Enabled = true;
                    MessageBox.Show(@"CWM 6.0.2.7 for the AT&T HTC One X+ has been flashed!", @"Hurray for CWM!",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (AndroidLib.Selector == "customRecovery")
                {
                    customRecoveryButton.Enabled = true;
                    MessageBox.Show(openFileDialog1.SafeFileName + @" has been flashed!",
                        @"Hurray for Custom Recoveries!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (AndroidLib.Selector == "flashKernel")
                {
                    flashKernelButton.Enabled = true;
                    MessageBox.Show(openFileDialog1.SafeFileName + @" has been flashed!", @"Hurray for Kernels!",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (AndroidLib.Selector == "flashSystem")
                {
                    flashKernelButton.Enabled = true;
                    MessageBox.Show(openFileDialog1.SafeFileName + @" has been flashed!", @"Hurray for Systems!",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (AndroidLib.Selector == "flashData")
                {
                    flashKernelButton.Enabled = true;
                    MessageBox.Show(openFileDialog1.SafeFileName + @" has been flashed!", @"Hurray for Datas!",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (AndroidLib.Selector == "flashTempKernel")
                {
                    temporaryKernelButton.Enabled = false;
                    MessageBox.Show(openFileDialog1.SafeFileName + @" has been temporarily flashed!", @"Hurray for Kernels!",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (AndroidLib.Selector == "flashTempRecovery")
                {
                    temporaryKernelButton.Enabled = false;
                    MessageBox.Show(openFileDialog1.SafeFileName + @" has been temporarily flashed!", @"Hurray for Recoveries!",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (AndroidLib.Selector == "writeSuperCID")
                {
                    temporaryKernelButton.Enabled = false;
                    MessageBox.Show(@"Your CID has been successfully changed!", @"Hurray for SuperCID!",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (AndroidLib.Selector == "writeIMEI")
                {
                    temporaryKernelButton.Enabled = false;
                    MessageBox.Show(@"Your IMEI has been successfully changed!", @"Hurray for IMEI!",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void tokenID_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                loadingSpinner.Visible = true;
                using (var sw = File.CreateText("./Data/token.txt"))
                {
                    sw.WriteLine(
                        Fastboot.ExecuteFastbootCommand(Fastboot.FormFastbootCommand(_device, AndroidLib.InitialCMD,
                            AndroidLib.SecondaryCMD)));
                    sw.WriteLine(
                        "PLEASE COPY EVERYTHING FROM <<<< Indentifier Token Start >>>> TO <<<< Indentifier Token End >>>>! YOU WILL NEED IT FOR THE NEXT STEP!");
                    sw.WriteLine("PLEASE ENSURE THAT YOU DELETE ALL (bootloader)'s AS WELL!");
                    sw.WriteLine("THIS FILE IS SAVED AS token.txt WITHIN THE DATA FOLDER IF NEEDED FOR FUTURE USE!");
                    sw.WriteLine("YOU MAY NOW MOVE ON TO THE SUBMIT TOKEN ID STEP!");
                }
                Process.Start(Application.StartupPath + "/Data/token.txt");
                _android.Dispose();
                loadingSpinner.Visible = false;
                getTokenIDButton.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void tokenID_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                refreshSpinner.Visible = true;
                deviceRecognition.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            try
            {
                refreshButton.Enabled = false;
                refreshSpinner.Visible = true;
                deviceRecognition.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void getTokenIDButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (statusLabelChange.Text == @"Fastboot")
                {
                    var dialogResult =
                        MessageBox.Show(
                            @"This is the first step in unlocking your bootloader." + "\n" + "\n" +
                            @"This will retrieve your Token ID." + "\n" + "\n" +
                            @"Once the process has completed, a text file will open with your Token ID and further instructions." +
                            "\n" + "\n" + @"Are you ready to continue?", @"Get Token ID", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information);
                    if (dialogResult == DialogResult.Yes)
                    {
                        loadingSpinner.Visible = true;
                        getTokenIDButton.Enabled = false;
                        AndroidLib.InitialCMD = "oem";
                        AndroidLib.SecondaryCMD = "get_identifier_token";
                        tokenID.RunWorkerAsync();
                    }
                }
                else if (statusLabelChange.Text == @"Online")
                {
                    var dialogResult =
                        MessageBox.Show(
                            @"This is the first step in unlocking your bootloader." + "\n" + "\n" +
                            @"This will retrieve your Token ID." + "\n" + "\n" +
                            @"Your phone will reboot into Fastboot mode, at which point a text file will open with your Token ID and further instructions." +
                            "\n" + "\n" + @"Are you ready to continue?", @"Get Token ID", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information);
                    if (dialogResult == DialogResult.Yes)
                    {
                        loadingSpinner.Visible = true;
                        getTokenIDButton.Enabled = false;
                        AndroidLib.InitialCMD = "reboot bootloader";
                        AndroidLib.Selector = "tokenID";
                        noReturnADBCommand.RunWorkerAsync();
                    }
                }
                else
                {
                    MessageBox.Show(
                        @"A phone has not been recognized by the toolkit! Please click the Reload button to check again!",
                        @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void submitTokenIDButton_Click(object sender, EventArgs e)
        {
            try
            {
                var dialogResult =
                    MessageBox.Show(
                        @"Please sign in to your HTC Dev account. If you do not have one, create and activate an account with a valid email address, then come back to this link. Afterwards, take the Token ID you retrieved before and paste it into the box at the bottom of the page. Hit submit, and wait for the email with the unlock binary file. Once you have recieved it, proceed to the next step. Are you ready to continue?",
                        @"Token ID Submit", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dialogResult == DialogResult.Yes)
                {
                    Process.Start("http://www.htcdev.com/bootloader/unlock-instructions/page-3");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void unlockBootloaderButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (statusLabelChange.Text == @"Fastboot")
                {
                    var dialogResult =
                        MessageBox.Show(
                            @"This will unlock your bootloader and completely wipe your phone." + "\n" + "\n" +
                            @"You must have received the unlock_code.bin file from HTC in your email, and have it downloaded and ready to be used." +
                            "\n" + "\n" + @"Have you backed up all necessary files and are ready to continue?",
                            @"Ready To Unlock?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                    if (dialogResult == DialogResult.Yes)
                    {
                        openFileDialog1.InitialDirectory = @"C:\";
                        openFileDialog1.Title = @"Select the binary file sent to you by HTC.";
                        openFileDialog1.FileName = "Choose unlock_code.bin...";
                        openFileDialog1.CheckFileExists = true;
                        openFileDialog1.CheckPathExists = true;
                        openFileDialog1.Filter = @" .BIN|*.bin";
                        if (openFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            loadingSpinner.Visible = true;
                            AndroidLib.InitialCMD = "flash";
                            AndroidLib.SecondaryCMD = "unlocktoken " + openFileDialog1.FileName;
                            AndroidLib.Selector = "bootloaderUnlock";
                            noReturnFastbootCommand.RunWorkerAsync();
                        }
                    }
                }
                else if (statusLabelChange.Text == @"Online")
                {
                    var dialogResult =
                        MessageBox.Show(
                            @"This will unlock your bootloader and completely wipe your phone." + "\n" + "\n" +
                            @"You must have received the unlock_code.bin file from HTC in your email, and have it downloaded and ready to be used." +
                            "\n" + "\n" + @"Have you backed up all necessary files and are ready to continue?",
                            @"Ready To Unlock?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                    if (dialogResult == DialogResult.Yes)
                    {
                        openFileDialog1.InitialDirectory = @"C:\";
                        openFileDialog1.Title = @"Select the binary file sent to you by HTC.";
                        openFileDialog1.FileName = "Choose unlock_code.bin...";
                        openFileDialog1.CheckFileExists = true;
                        openFileDialog1.CheckPathExists = true;
                        openFileDialog1.Filter = @" .BIN|*.bin";
                        if (openFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            loadingSpinner.Visible = true;
                            AndroidLib.InitialCMD = "reboot bootloader";
                            AndroidLib.Selector = "bootloaderUnlock";
                            noReturnADBCommand.RunWorkerAsync();
                        }
                    }
                }
                else
                {
                    MessageBox.Show(
                        @"A phone has not been recognized by the toolkit! Please click the Reload button to check again!",
                        @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void internationalTWRPButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists("./Data/Recoveries/TWRPINT.img"))
                {
                    if (statusLabelChange.Text == @"Fastboot")
                    {
                        loadingSpinner.Visible = true;
                        internationalTWRPButton.Enabled = false;
                        AndroidLib.InitialCMD = "flash";
                        AndroidLib.SecondaryCMD = "recovery ./Data/Recoveries/TWRPINT.img";
                        AndroidLib.Selector = "internationalTWRP";
                        noReturnFastbootCommand.RunWorkerAsync();
                    }
                    else if (statusLabelChange.Text == @"Online")
                    {
                        loadingSpinner.Visible = true;
                        internationalTWRPButton.Enabled = false;
                        AndroidLib.InitialCMD = "reboot bootloader";
                        AndroidLib.Selector = "internationalTWRP";
                        noReturnADBCommand.RunWorkerAsync();
                    }
                    else
                    {
                        MessageBox.Show(
                            @"A phone has not been recognized by the toolkit! Please click the Reload button to check again!",
                            @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show(@"This recovery appears to be missing from the Data folder!",
                        @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void internationalCWMButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists("./Data/Recoveries/CWMINT.img"))
                {
                    if (statusLabelChange.Text == @"Fastboot")
                    {
                        loadingSpinner.Visible = true;
                        internationalCWMButton.Enabled = false;
                        AndroidLib.InitialCMD = "flash";
                        AndroidLib.SecondaryCMD = "recovery ./Data/Recoveries/CWMINT.img";
                        AndroidLib.Selector = "internationalCWM";
                        noReturnFastbootCommand.RunWorkerAsync();
                    }
                    else if (statusLabelChange.Text == @"Online")
                    {
                        loadingSpinner.Visible = true;
                        internationalCWMButton.Enabled = false;
                        AndroidLib.InitialCMD = "reboot bootloader";
                        AndroidLib.Selector = "internationalCWM";
                        noReturnADBCommand.RunWorkerAsync();
                    }
                    else
                    {
                        MessageBox.Show(
                            @"A phone has not been recognized by the toolkit! Please click the Reload button to check again!",
                            @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show(@"This recovery appears to be missing from the Data folder!",
                        @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void attTWRPButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists("./Data/Recoveries/TWRPATT.img"))
                {
                    if (statusLabelChange.Text == @"Fastboot")
                    {
                        loadingSpinner.Visible = true;
                        attTWRPButton.Enabled = false;
                        AndroidLib.InitialCMD = "flash";
                        AndroidLib.SecondaryCMD = "recovery ./Data/Recoveries/TWRPATT.img";
                        AndroidLib.Selector = "attTWRP";
                        noReturnFastbootCommand.RunWorkerAsync();
                    }
                    else if (statusLabelChange.Text == @"Online")
                    {
                        loadingSpinner.Visible = true;
                        attTWRPButton.Enabled = false;
                        AndroidLib.InitialCMD = "reboot bootloader";
                        AndroidLib.Selector = "attTWRP";
                        noReturnADBCommand.RunWorkerAsync();
                    }
                    else
                    {
                        MessageBox.Show(
                            @"A phone has not been recognized by the toolkit! Please click the Reload button to check again!",
                            @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show(@"This recovery appears to be missing from the Data folder!",
                        @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void attCWMButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists("./Data/Recoveries/CWMATT.img"))
                {
                    if (statusLabelChange.Text == @"Fastboot")
                    {
                        loadingSpinner.Visible = true;
                        attCWMButton.Enabled = false;
                        AndroidLib.InitialCMD = "flash";
                        AndroidLib.SecondaryCMD = "recovery ./Data/Recoveries/CWMATT.img";
                        AndroidLib.Selector = "attCWM";
                        noReturnFastbootCommand.RunWorkerAsync();
                    }
                    else if (statusLabelChange.Text == @"Online")
                    {
                        loadingSpinner.Visible = true;
                        attCWMButton.Enabled = false;
                        AndroidLib.InitialCMD = "reboot bootloader";
                        AndroidLib.Selector = "attCWM";
                        noReturnADBCommand.RunWorkerAsync();
                    }
                    else
                    {
                        MessageBox.Show(
                            @"A phone has not been recognized by the toolkit! Please click the Reload button to check again!",
                            @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show(@"This recovery appears to be missing from the Data folder!",
                        @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void customRecoveryButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (statusLabelChange.Text == @"Fastboot")
                {
                    var dialogResult =
                        MessageBox.Show(
                            @"This will allow you to flash a custom Recovery .img if you do not want to use the choices given in the toolkit. Are you ready to continue?",
                            @"Custom Recovery Flash", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (dialogResult == DialogResult.Yes)
                    {
                        openFileDialog1.InitialDirectory = @"C:\";
                        openFileDialog1.Title = @"Please select a Recovery .img file";
                        openFileDialog1.FileName = "Choose File...";
                        openFileDialog1.CheckFileExists = true;
                        openFileDialog1.CheckPathExists = true;
                        openFileDialog1.Filter = @" .IMG|*.img";
                        if (openFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            loadingSpinner.Visible = true;
                            customRecoveryButton.Enabled = false;
                            AndroidLib.InitialCMD = "flash";
                            AndroidLib.SecondaryCMD = "recovery " + openFileDialog1.FileName;
                            AndroidLib.Selector = "customRecovery";
                            noReturnFastbootCommand.RunWorkerAsync();
                        }
                    }
                }
                else if (statusLabelChange.Text == @"Online")
                {
                    var dialogResult =
                        MessageBox.Show(
                            @"This will allow you to flash a custom Recovery .img if you do not want to use the choices given in the toolkit. Are you ready to continue?",
                            @"Custom Recovery Flash", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (dialogResult == DialogResult.Yes)
                    {
                        openFileDialog1.InitialDirectory = @"C:\";
                        openFileDialog1.Title = @"Please select a Recovery .img file";
                        openFileDialog1.FileName = "Choose File...";
                        openFileDialog1.CheckFileExists = true;
                        openFileDialog1.CheckPathExists = true;
                        openFileDialog1.Filter = @" .IMG|*.img";
                        if (openFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            loadingSpinner.Visible = true;
                            customRecoveryButton.Enabled = false;
                            AndroidLib.InitialCMD = "reboot bootloader";
                            AndroidLib.Selector = "customRecovery";
                            noReturnADBCommand.RunWorkerAsync();
                        }
                    }
                }
                else
                {
                    MessageBox.Show(
                        @"A phone has not been recognized by the toolkit! Please click the Reload button to check again!",
                        @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void pushFile_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (_device.PushFile(AndroidLib.InitialCMD, AndroidLib.SecondaryCMD).ToString() == "True")
                {
                    if (AndroidLib.Selector == "superuser")
                    {
                        AndroidLib.InitialCMD = "reboot recovery";
                        noReturnADBCommand.RunWorkerAsync();
                        MessageBox.Show(
                            @"Superuser was successfully pushed! Your phone will now reboot into Recovery.",
                            @"Superuser Push Successful!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        flashSuperuserButton.Enabled = true;
                    }
                    else if (AndroidLib.Selector == "flashROM")
                    {
                        AndroidLib.InitialCMD = "reboot recovery";
                        noReturnADBCommand.RunWorkerAsync();
                        MessageBox.Show(
                            openFileDialog1.SafeFileName +
                            @" was successfully pushed! Your phone will now reboot into Recovery.",
                            @"ROM Push Successful!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        flashROMButton.Enabled = true;
                    }
                    else if (AndroidLib.Selector == "systemApp")
                    {
                        AndroidLib.InitialCMD = "chmod";
                        AndroidLib.SecondaryCMD = "644 /system/app/" + openFileDialog1.SafeFileName;
                        AndroidLib.Selector = "systemApp";
                        adbShellCommand.RunWorkerAsync();
                    }
                    else
                    {
                        loadingSpinner.Visible = false;
                        pushFilesButton.Enabled = true;
                        MessageBox.Show(openFileDialog1.SafeFileName + @" was successfully pushed!",
                            @"File Push Successful!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    _android.Dispose();
                }
                else
                {
                    if (AndroidLib.Selector == "superuser")
                    {
                        loadingSpinner.Visible = false;
                        MessageBox.Show(
                            @"An error occured while attempting to push Superuser.zip. Please try again in a few moments.",
                            @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        flashSuperuserButton.Enabled = true;
                    }
                    else if (AndroidLib.Selector == "systemApp")
                    {
                        loadingSpinner.Visible = false;
                        MessageBox.Show(
                            @"An error occured while attempting to install " + openFileDialog1.SafeFileName +
                            @" as a system app. " + @"Please try again in a few moments.",
                            @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        installSystemAppButton.Enabled = true;
                    }
                    else
                    {
                        loadingSpinner.Visible = false;
                        MessageBox.Show(
                            @"An error occured while attempting to push " + openFileDialog1.SafeFileName + @". " +
                            @"Please try again in a few moments.", @"Houston, we have a problem!", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        pushFilesButton.Enabled = true;
                    }
                    _android.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void flashSuperuserButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (statusLabelChange.Text == @"Online")
                {
                    if (File.Exists("./Data/Installers/Superuser.zip"))
                    {
                        var dialogResult =
                            MessageBox.Show(
                                @"This will push Superuser.zip to your phone and boot you into Recovery." + "\n" + "\n" +
                                @"Once there, flash the Superuser.zip like any other file." + "\n" + "\n" +
                                @"Are you ready to continue?", @"Flash Superuser", MessageBoxButtons.YesNo,
                                MessageBoxIcon.Information);
                        if (dialogResult == DialogResult.Yes)
                        {
                            loadingSpinner.Visible = true;
                            AndroidLib.InitialCMD = "./Data/Installers/Superuser.zip";
                            AndroidLib.SecondaryCMD = "/sdcard/Superuser.zip";
                            AndroidLib.Selector = "superuser";
                            pushFile.RunWorkerAsync();
                            flashSuperuserButton.Enabled = false;
                        }
                    }
                    else
                    {
                        var dialogResult =
                            MessageBox.Show(
                                @"Superuser.zip appears to have not dowloaded correctly! Would you like to try again?",
                                @"Houston, we have a problem!", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        if (dialogResult == DialogResult.Yes)
                        {
                            downloadFiles.RunWorkerAsync();
                        }
                    }
                }
                else
                {
                    MessageBox.Show(
                        @"A phone has not been recognized by the toolkit! Please click the Reload button to check again!",
                        @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void flashROMButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (statusLabelChange.Text == @"Online")
                {
                    var dialogResult =
                        MessageBox.Show(
                            @"This will push a ROM .zip file of your choosing to your phone and boot you into Recovery. Once there, flash the ROM .zip like any other file." +
                            "\n" + "\n" +
                            @"Afterwards, use the 'Flash Kernel' option to flash the boot.img that came with your ROM." +
                            "\n" + "\n" + @"Are you ready to continue?", @"Flash ROM", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information);
                    if (dialogResult == DialogResult.Yes)
                    {
                        openFileDialog1.InitialDirectory = @"C:\";
                        openFileDialog1.Title = @"Select your ROM .zip file.";
                        openFileDialog1.FileName = "Choose File...";
                        openFileDialog1.CheckFileExists = true;
                        openFileDialog1.CheckPathExists = true;
                        openFileDialog1.Filter = @" .ZIP|*.zip";
                        if (openFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            loadingSpinner.Visible = true;
                            flashROMButton.Enabled = false;
                            AndroidLib.InitialCMD = openFileDialog1.FileName;
                            AndroidLib.SecondaryCMD = "/sdcard/" + openFileDialog1.SafeFileName;
                            AndroidLib.Selector = "flashROM";
                            pushFile.RunWorkerAsync();
                        }
                    }
                }
                else
                {
                    MessageBox.Show(
                        @"A phone has not been recognized by the toolkit! Please click the Reload button to check again!",
                        @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void flashKernelButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (statusLabelChange.Text == @"Fastboot")
                {
                    var dialogResult =
                        MessageBox.Show(
                            @"This will allow you to flash a custom Kernel .img. Are you ready to continue?",
                            @"Custom Recovery Flash", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (dialogResult == DialogResult.Yes)
                    {
                        openFileDialog1.InitialDirectory = @"C:\";
                        openFileDialog1.Title = @"Please select a Kernel .img file";
                        openFileDialog1.FileName = "Choose File...";
                        openFileDialog1.CheckFileExists = true;
                        openFileDialog1.CheckPathExists = true;
                        openFileDialog1.Filter = @" .IMG|*.img";
                        if (openFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            loadingSpinner.Visible = true;
                            flashKernelButton.Enabled = false;
                            AndroidLib.InitialCMD = "flash";
                            AndroidLib.SecondaryCMD = "boot " + openFileDialog1.FileName;
                            AndroidLib.Selector = "flashKernel";
                            noReturnFastbootCommand.RunWorkerAsync();
                        }
                    }
                }
                else if (statusLabelChange.Text == @"Online")
                {
                    var dialogResult =
                        MessageBox.Show(
                            @"This will allow you to flash a custom Kernel .img. Are you ready to continue?",
                            @"Custom Recovery Flash", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (dialogResult == DialogResult.Yes)
                    {
                        openFileDialog1.InitialDirectory = @"C:\";
                        openFileDialog1.Title = @"Please select a Kernel .img file";
                        openFileDialog1.FileName = "Choose File...";
                        openFileDialog1.CheckFileExists = true;
                        openFileDialog1.CheckPathExists = true;
                        openFileDialog1.Filter = @" .IMG|*.img";
                        if (openFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            loadingSpinner.Visible = true;
                            flashKernelButton.Enabled = false;
                            AndroidLib.InitialCMD = "reboot bootloader";
                            AndroidLib.Selector = "flashKernel";
                            noReturnADBCommand.RunWorkerAsync();
                        }
                    }
                }
                else
                {
                    MessageBox.Show(
                        @"A phone has not been recognized by the toolkit! Please click the Reload button to check again!",
                        @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void installApp_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (_device.InstallApk(AndroidLib.InitialCMD).ToString() == "True")
                {
                    if (AndroidLib.Selector == "busybox")
                    {
                        loadingSpinner.Visible = false;
                        installBusyboxButton.Enabled = true;
                        MessageBox.Show(@"Busybox was successfully installed!", @"Hurray for Busybox!",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    if (AndroidLib.Selector == "xposed")
                    {
                        loadingSpinner.Visible = false;
                        installXposedButton.Enabled = false;
                        MessageBox.Show(@"Xposed Framework was successfully installed!", @"Hurray for Xposed!",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        loadingSpinner.Visible = false;
                        installAppButton.Enabled = true;
                        MessageBox.Show(openFileDialog1.SafeFileName + @" was successfully installed!",
                            @"Hurray for Apps!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    _android.Dispose();
                }
                else
                {
                    if (AndroidLib.Selector == "busybox")
                    {
                        loadingSpinner.Visible = false;
                        installBusyboxButton.Enabled = true;
                        MessageBox.Show(
                            @"An issue occured while attempting to install Busybox! Please try again in a few moments.",
                            @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    if (AndroidLib.Selector == "xposed")
                    {
                        loadingSpinner.Visible = false;
                        installXposedButton.Enabled = false;
                        MessageBox.Show(
                            @"An issue occured while attempting to install Xposed Framework! Please try again in a few moments.",
                            @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        loadingSpinner.Visible = false;
                        installAppButton.Enabled = true;
                        MessageBox.Show(
                            @"An issue occured while attempting to install " + openFileDialog1.SafeFileName +
                            @". Please try again in a few moments.", @"Houston, we have a problem!",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    _android.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void installBusyboxButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists("./Data/Installers/Busybox.apk"))
                {
                    if (statusLabelChange.Text == @"Online")
                    {
                        var dialogResult =
                            MessageBox.Show(
                                @"This will install Busybox to your phone. This is a framework that is necessary for many apps to run correctly." +
                                "\n" + "\n" + @"This is not necessary if your ROM comes with Busybox support built in." +
                                "\n" + "\n" + @"Would you like to install it now?", @"Install Busybox",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (dialogResult == DialogResult.Yes)
                        {
                            loadingSpinner.Visible = true;
                            AndroidLib.InitialCMD = "./Data/Installers/Busybox.apk";
                            AndroidLib.Selector = "busybox";
                            installApp.RunWorkerAsync();
                            installBusyboxButton.Enabled = false;
                        }
                    }
                    else
                    {
                        MessageBox.Show(
                            @"A phone has not been recognized by the toolkit! Please click the Reload button to check again!",
                            @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    var dialogResult =
                        MessageBox.Show(
                            @"Busybox appears to have not dowloaded correctly! Would you like to try again?",
                            @"Houston, we have a problem!", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    if (dialogResult == DialogResult.Yes)
                    {
                        downloadFiles.RunWorkerAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void installXposedButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists("./Data/Installers/Xposed.apk"))
                {
                    if (statusLabelChange.Text == @"Online")
                    {
                        var dialogResult =
                            MessageBox.Show(
                                @"This will install the Xposed Framework to your phone." + "\n" + "\n" +
                                @"This will allow you to make lots of changes to your phone's appearance without editing any ROM files." +
                                "\n" + "\n" + @"Would you like to install it now?", @"Install Xposed Framework",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (dialogResult == DialogResult.Yes)
                        {
                            loadingSpinner.Visible = true;
                            AndroidLib.InitialCMD = "./Data/Installers/Xposed.apk";
                            AndroidLib.Selector = "xposed";
                            installApp.RunWorkerAsync();
                            installXposedButton.Enabled = false;
                        }
                    }
                    else
                    {
                        MessageBox.Show(
                            @"A phone has not been recognized by the toolkit! Please click the Reload button to check again!",
                            @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    var dialogResult =
                        MessageBox.Show(
                            @"Xposed Framework appears to have not dowloaded correctly! Would you like to try again?",
                            @"Houston, we have a problem!", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    if (dialogResult == DialogResult.Yes)
                    {
                        downloadFiles.RunWorkerAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void deviceReboot_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (AndroidLib.Selector == "reboot")
                {
                    _device.Reboot();
                    loadingSpinner.Visible = false;
                    rebootButton.Enabled = true;
                }
                else if (AndroidLib.Selector == "rebootRecovery")
                {
                    _device.RebootRecovery();
                    rebootRecoveryButton.Enabled = true;
                    loadingSpinner.Visible = false;
                }
                else if (AndroidLib.Selector == "rebootBootloader")
                {
                    _device.RebootBootloader();
                    rebootBootloaderButton.Enabled = true;
                    loadingSpinner.Visible = false;
                }
                else if (AndroidLib.Selector == "rebootFastboot")
                {
                    _device.FastbootReboot();
                    rebootBootloaderButton.Enabled = true;
                    loadingSpinner.Visible = false;
                }
                else if (AndroidLib.Selector == "rebootRecoverySideload")
                {
                    sideloadROMButton.Enabled = false;
                    _device.RebootRecovery();
                    MessageBox.Show(
                        @"Your phone is now rebooting into Recovery." + "\n" + "\n" + @"DO NOT PRESS OKAY YET." + "\n" +
                        "\n" +
                        @"Wait until your recovery has completely loaded, then open the ADB Sideload option and let it initialize." +
                        "\n" + "\n" + @"If you are using TWRP, it will be under Advanced." + "\n" + "\n" +
                        @"Once it has fully initialized, click OK to begin flashing your chosen .zip.", @"ADB Sideload",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    AndroidLib.InitialCMD = "sideload ";
                    AndroidLib.SecondaryCMD = openFileDialog1.FileName;
                    noReturnADBCommand.RunWorkerAsync();
                }
                else if (AndroidLib.Selector == "getSerial")
                {
                    fastbootInformationTextBox.Text = _device.SerialNumber;
                    getSerialNumberButton.Enabled = true;
                    loadingSpinner.Visible = false;
                }
                _android.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void rebootButton_Click(object sender, EventArgs e)
        {
            if (statusLabelChange.Text == @"Online")
            {
                try
                {
                    loadingSpinner.Visible = true;
                    rebootButton.Enabled = false;
                    AndroidLib.Selector = "reboot";
                    deviceReboot.RunWorkerAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                        @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                    var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                    file.WriteLine(ex);
                    file.Close();
                }
            }
            else
            {
                MessageBox.Show(
                    @"A phone has not been recognized by the toolkit! Please click the Reload button to check again!",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rebootRecoveryButton_Click(object sender, EventArgs e)
        {
            if (statusLabelChange.Text == @"Online")
            {
                try
                {
                    loadingSpinner.Visible = true;
                    rebootRecoveryButton.Enabled = false;
                    AndroidLib.Selector = "rebootRecovery";
                    deviceReboot.RunWorkerAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                        @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                    var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                    file.WriteLine(ex);
                    file.Close();
                }
            }
            else
            {
                MessageBox.Show(
                    @"A phone has not been recognized by the toolkit! Please click the Reload button to check again!",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rebootBootloaderButton_Click(object sender, EventArgs e)
        {
            if (statusLabelChange.Text == @"Online")
            {
                try
                {
                    loadingSpinner.Visible = true;
                    rebootBootloaderButton.Enabled = false;
                    AndroidLib.Selector = "rebootBootloader";
                    deviceReboot.RunWorkerAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                        @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                    var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                    file.WriteLine(ex);
                    file.Close();
                }
            }
            else
            {
                MessageBox.Show(
                    @"A phone has not been recognized by the toolkit! Please click the Reload button to check again!",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void adbCommand_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Adb.ExecuteAdbCommand(Adb.FormAdbCommand(_device, AndroidLib.InitialCMD, AndroidLib.SecondaryCMD));
                loadingSpinner.Visible = false;
                backupButton.Enabled = true;
                _android.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void sideloadROMButton_Click(object sender, EventArgs e)
        {
            try
            {
                var dialogResult =
                    MessageBox.Show(
                        @"This will push a ROM or other .zip file to your phone and automatically install it." + "\n" +
                        "\n" + @"This is best used when only the recovery is accessible on your phone." + "\n" + "\n" +
                        @"Are you currently in ADB Sideload mode now?", @"ADB Sideload", MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Information);
                if (dialogResult == DialogResult.Yes)
                {
                    openFileDialog1.InitialDirectory = @"C:\";
                    openFileDialog1.Title = @"Select the .zip file you would like to flash in ADB Sideload";
                    openFileDialog1.FileName = "Choose File...";
                    openFileDialog1.Filter = @" .ZIP|*.zip";
                    openFileDialog1.CheckFileExists = true;
                    openFileDialog1.CheckPathExists = true;
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        loadingSpinner.Visible = true;
                        sideloadROMButton.Enabled = false;
                        AndroidLib.InitialCMD = "sideload";
                        AndroidLib.SecondaryCMD = openFileDialog1.FileName;
                        adbCommand.RunWorkerAsync();
                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                    openFileDialog1.InitialDirectory = @"C:\";
                    openFileDialog1.Title = @"Select the .zip file you would like to sideloaded";
                    openFileDialog1.FileName = "Choose File...";
                    openFileDialog1.Filter = @" .ZIP|*.zip";
                    openFileDialog1.CheckFileExists = true;
                    openFileDialog1.CheckPathExists = true;
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        loadingSpinner.Visible = true;
                        sideloadROMButton.Enabled = false;
                        AndroidLib.Selector = "rebootRecoverySideload";
                        deviceReboot.RunWorkerAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void pushFilesButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (statusLabelChange.Text == @"Online")
                {
                    if (sdCardCheckBox.Checked)
                    {
                        openFileDialog1.InitialDirectory = @"C:\";
                        openFileDialog1.Title = @"Select a file";
                        openFileDialog1.FileName = "Choose File...";
                        openFileDialog1.CheckFileExists = true;
                        openFileDialog1.CheckPathExists = true;
                        if (openFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            loadingSpinner.Visible = true;
                            pushFilesButton.Enabled = false;
                            AndroidLib.InitialCMD = openFileDialog1.FileName;
                            AndroidLib.SecondaryCMD = "/sdcard/" + openFileDialog1.SafeFileName;
                            pushFile.RunWorkerAsync();
                        }
                    }
                    else if (sdCardCheckBox.Checked == false)
                    {
                        openFileDialog1.InitialDirectory = @"C:\";
                        openFileDialog1.Title = @"Select a file";
                        openFileDialog1.FileName = "Choose File...";
                        openFileDialog1.CheckFileExists = true;
                        openFileDialog1.CheckPathExists = true;
                        if (openFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            loadingSpinner.Visible = true;
                            pushFilesButton.Enabled = false;
                            var input =
                                Interaction.InputBox(
                                    "Please input the exact location you want the file to go to. Ensure you put an ending slash (/) at the end of the location. If you are not sure what this means, please check the 'SD?' checkbox in the toolkit.",
                                    "Location", "", 780, 450);
                            AndroidLib.InitialCMD = openFileDialog1.FileName;
                            AndroidLib.SecondaryCMD = input + openFileDialog1.SafeFileName;
                            pushFile.RunWorkerAsync();
                        }
                    }
                }
                else
                {
                    MessageBox.Show(
                        @"A phone has not been recognized by the toolkit! Please click the Reload button to check again!",
                        @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void pullFilesButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (statusLabelChange.Text == @"Online")
                {
                    saveFileDialog1.InitialDirectory = @"C:\";
                    saveFileDialog1.Title =
                        @"Choose a name for your file with a file extension (.png, .apk, etc) that matches the file on your phone.";
                    saveFileDialog1.FileName = "Save your File...";
                    saveFileDialog1.CheckPathExists = true;
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        var location =
                            Interaction.InputBox(
                                "Please input the EXACT location of the file within your phone. For example, if you wanted to pull a specific file off your main storage, you would put '/sdcard/SpecificFile.file', without quotes. DO NOT PUT AN ENDING SLASH (/) IN THE LOCATION!",
                                "Location", "", 775, 450);
                        loadingSpinner.Visible = true;
                        pullFilesButton.Enabled = false;
                        AndroidLib.InitialCMD = "pull";
                        AndroidLib.SecondaryCMD = location + " " + saveFileDialog1.FileName;
                        adbCommand.RunWorkerAsync();
                    }
                }
                else
                {
                    MessageBox.Show(
                        @"A phone has not been recognized by the toolkit! Please click the Reload button to check again!",
                        @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void systemRemountButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (statusLabelChange.Text == @"Online")
                {
                    loadingSpinner.Visible = true;
                    systemRemountButton.Enabled = false;
                    AndroidLib.InitialCMD = "remount";
                    AndroidLib.Selector = "systemRemount";
                    noReturnADBCommand.RunWorkerAsync();
                }
                else
                {
                    MessageBox.Show(
                        @"A phone has not been recognized by the toolkit! Please click the Reload button to check again!",
                        @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void adbShellCommand_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Adb.ExecuteAdbCommand(Adb.FormAdbShellCommand(_device, true, AndroidLib.InitialCMD,
                    AndroidLib.SecondaryCMD));
                _android.Dispose();
                loadingSpinner.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void adbShellCommand_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (AndroidLib.Selector == "freeRAM")
                {
                    loadingSpinner.Visible = false;
                    freeRAMButton.Enabled = true;
                    MessageBox.Show(@"RAM has been successfully freed!", @"Freedom RAM!", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else if (AndroidLib.Selector == "systemApp")
                {
                    loadingSpinner.Visible = false;
                    installSystemAppButton.Enabled = true;
                    MessageBox.Show(openFileDialog1.SafeFileName + @" was successfully installed!",
                        @"Hurray for System Apps!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void freeRAMButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (statusLabelChange.Text == @"Online")
                {
                    loadingSpinner.Visible = true;
                    freeRAMButton.Enabled = false;
                    AndroidLib.InitialCMD = "am";
                    AndroidLib.SecondaryCMD = "kill-all";
                    AndroidLib.Selector = "freeRAM";
                    adbShellCommand.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void installAppButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (statusLabelChange.Text == @"Online")
                {
                    openFileDialog1.InitialDirectory = @"C:\";
                    openFileDialog1.Title = @"Select a valid Android app file (.apk)";
                    openFileDialog1.FileName = "Choose File...";
                    openFileDialog1.CheckFileExists = true;
                    openFileDialog1.CheckPathExists = true;
                    openFileDialog1.Filter = @" .APK|*.apk";
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        loadingSpinner.Visible = true;
                        AndroidLib.InitialCMD = openFileDialog1.FileName;
                        installApp.RunWorkerAsync();
                        installAppButton.Enabled = false;
                    }
                }
                else
                {
                    MessageBox.Show(
                        @"A phone has not been recognized by the toolkit! Please click the Reload button to check again!",
                        @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void installSystemAppButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (statusLabelChange.Text == @"Online")
                {
                    openFileDialog1.InitialDirectory = @"C:\";
                    openFileDialog1.Title = @"Select a valid Android app file (.apk)";
                    openFileDialog1.FileName = "Choose File...";
                    openFileDialog1.CheckFileExists = true;
                    openFileDialog1.CheckPathExists = true;
                    openFileDialog1.Filter = @" .APK|*.apk";
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        loadingSpinner.Visible = true;
                        installSystemAppButton.Enabled = false;
                        AndroidLib.InitialCMD = "remount";
                        AndroidLib.Selector = "systemApp";
                        noReturnADBCommand.RunWorkerAsync();
                    }
                }
                else
                {
                    MessageBox.Show(
                        @"A phone has not been recognized by the toolkit! Please click the Reload button to check again!",
                        @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void uninstallAppButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (statusLabelChange.Text == @"Online")
                {
                    var location =
                        Interaction.InputBox(
                            "Please input the name of the APK package for the app you want to uninstall. For example, Adobe Reader's package file is 'com.adobe.reader'.",
                            "Uninstall App", "", 775, 450);
                    loadingSpinner.Visible = true;
                    pullFilesButton.Enabled = false;
                    AndroidLib.InitialCMD = "uninstall";
                    AndroidLib.SecondaryCMD = location;
                    adbCommand.RunWorkerAsync();
                }
                else
                {
                    MessageBox.Show(
                        @"A phone has not been recognized by the toolkit! Please click the Reload button to check again!",
                        @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void getLogcat_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                using (var sw = File.CreateText("./Data/Logcats/" + fileDateTime + ".txt"))
                {
                    sw.WriteLine(
                        Adb.ExecuteAdbCommand(Adb.FormAdbCommand(AndroidLib.InitialCMD, AndroidLib.SecondaryCMD)));
                }
                Process.Start(Application.StartupPath + "/Data/Logcats");
                _android.Dispose();
                loadingSpinner.Visible = false;
                logcatButton.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void logcatButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (statusLabelChange.Text == @"Online")
                {
                    loadingSpinner.Visible = true;
                    logcatButton.Enabled = false;
                    AndroidLib.InitialCMD = "logcat";
                    AndroidLib.SecondaryCMD = "-d";
                    getLogcat.RunWorkerAsync();
                }
                else
                {
                    MessageBox.Show(
                        @"A phone has not been recognized by the toolkit! Please click the Reload button to check again!",
                        @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void getDmesg_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                using (var sw = File.CreateText("./Data/Logcats/" + fileDateTime + "_DMESG.txt"))
                {
                    sw.WriteLine(Adb.ExecuteAdbCommand(Adb.FormAdbShellCommand(_device, true, AndroidLib.InitialCMD)));
                }
                Process.Start(Application.StartupPath + "/Data/Logcats");
                _android.Dispose();
                loadingSpinner.Visible = false;
                dmesgButton.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void dmesgButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (statusLabelChange.Text == @"Online")
                {
                    loadingSpinner.Visible = true;
                    dmesgButton.Enabled = false;
                    AndroidLib.InitialCMD = "dmesg";
                    getDmesg.RunWorkerAsync();
                }
                else
                {
                    MessageBox.Show(
                        @"A phone has not been recognized by the toolkit! Please click the Reload button to check again!",
                        @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void backupButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (statusLabelChange.Text == @"Online")
                {
                    var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                    var dialogResult =
                        MessageBox.Show(
                            @"This will create a full backup of the data on your phone." + "\n" + "\n" +
                            @"This method can be unreliable and does not guarantee all of your data being saved in the event of unlocking your bootloader, rooting, etc." +
                            "\n" + "\n" + @"This method only works with Android 4.0 and above." + "\n" + "\n" +
                            @"Are you ready to continue?", @"Phone Backup", MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information);
                    if (dialogResult == DialogResult.Yes)
                    {
                        loadingSpinner.Visible = true;
                        backupButton.Enabled = false;
                        AndroidLib.InitialCMD = "backup";
                        AndroidLib.SecondaryCMD = "-apk -all -f ./Data/Backups/" + fileDateTime + ".ab";
                        adbCommand.RunWorkerAsync();
                        MessageBox.Show(
                            @"A process will now open on your phone allowing you to password protect and continue with the backup process." +
                            "\n" + "\n" +
                            @"Please do not disturb your phone or the toolkit until the process completes." + "\n" +
                            "\n" + @"You may close this popup.", @"Phone Backup", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show(
                        @"A phone has not been recognized by the toolkit! Please click the Reload button to check again!",
                        @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void restoreButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (statusLabelChange.Text == @"Online")
                {
                    openFileDialog1.InitialDirectory = @"C:\";
                    openFileDialog1.Title = @"Please select an Android backup file (.ab)";
                    openFileDialog1.FileName = "Choose File...";
                    openFileDialog1.CheckFileExists = true;
                    openFileDialog1.CheckPathExists = true;
                    openFileDialog1.Filter = @" .AB|*.ab";
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        loadingSpinner.Visible = true;
                        restoreButton.Enabled = false;
                        AndroidLib.InitialCMD = "restore";
                        AndroidLib.SecondaryCMD = openFileDialog1.FileName;
                        adbCommand.RunWorkerAsync();
                    }
                }
                else
                {
                    MessageBox.Show(
                        @"A phone has not been recognized by the toolkit! Please click the Reload button to check again!",
                        @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void rebootToBootloaderButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (statusLabelChange.Text == @"Online")
                {
                    loadingSpinner.Visible = true;
                    rebootBootloaderButton.Enabled = false;
                    AndroidLib.Selector = "rebootBootloader";
                    deviceReboot.RunWorkerAsync();
                }
                else
                {
                    MessageBox.Show(
                        @"A phone has not been recognized by the toolkit! Please click the Reload button to check again!",
                        @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void rebootFromBootloaderButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (statusLabelChange.Text == @"Fastboot")
                {
                    loadingSpinner.Visible = true;
                    rebootBootloaderButton.Enabled = false;
                    AndroidLib.Selector = "rebootFastboot";
                    deviceReboot.RunWorkerAsync();
                }
                else
                {
                    MessageBox.Show(
                        @"A phone has not been recognized by the toolkit! Please click the Reload button to check again!",
                        @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void flashSystemImageButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (statusLabelChange.Text == @"Fastboot")
                {
                    openFileDialog1.InitialDirectory = @"C:\";
                    openFileDialog1.Title = @"Select your System .IMG";
                    openFileDialog1.FileName = "Choose File...";
                    openFileDialog1.CheckFileExists = true;
                    openFileDialog1.CheckPathExists = true;
                    openFileDialog1.Filter = @" .IMG|*.img";
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        loadingSpinner.Visible = true;
                        flashSystemImageButton.Enabled = false;
                        AndroidLib.InitialCMD = "flash";
                        AndroidLib.SecondaryCMD = "system " + openFileDialog1.FileName;
                        AndroidLib.Selector = "flashSystem";
                        noReturnFastbootCommand.RunWorkerAsync();
                    }
                }
                else
                {
                    MessageBox.Show(
                        @"A phone in Fastboot mode has not been recognized by the toolkit! Please click the Reload button to check again!",
                        @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void flashDataImageButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (statusLabelChange.Text == @"Fastboot")
                {
                    openFileDialog1.InitialDirectory = @"C:\";
                    openFileDialog1.Title = @"Select your System .IMG";
                    openFileDialog1.FileName = "Choose File...";
                    openFileDialog1.CheckFileExists = true;
                    openFileDialog1.CheckPathExists = true;
                    openFileDialog1.Filter = @" .IMG|*.img";
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        loadingSpinner.Visible = true;
                        flashSystemImageButton.Enabled = false;
                        AndroidLib.InitialCMD = "flash";
                        AndroidLib.SecondaryCMD = "data " + openFileDialog1.FileName;
                        AndroidLib.Selector = "flashData";
                        noReturnFastbootCommand.RunWorkerAsync();
                    }
                }
                else
                {
                    MessageBox.Show(
                        @"A phone in Fastboot mode has not been recognized by the toolkit! Please click the Reload button to check again!",
                        @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void getFastbootInfo_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                fastbootInformationTextBox.Text =
                    Fastboot.ExecuteFastbootCommand(Fastboot.FormFastbootCommand(_device, AndroidLib.InitialCMD,
                        AndroidLib.SecondaryCMD));
                getSerialNumberButton.Enabled = true;
                getIMEIButton.Enabled = true;
                getCIDButton.Enabled = true;
                getMIDButton.Enabled = true;
                loadingSpinner.Visible = false;
                _android.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void getSerialNumberButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (statusLabelChange.Text == @"Online")
                {
                    loadingSpinner.Visible = true;
                    getSerialNumberButton.Enabled = false;
                    AndroidLib.Selector = "getSerial";
                    deviceReboot.RunWorkerAsync();
                }
                else if (statusLabelChange.Text == @"Fastboot")
                {
                    loadingSpinner.Visible = true;
                    getSerialNumberButton.Enabled = false;
                    AndroidLib.InitialCMD = "getvar";
                    AndroidLib.SecondaryCMD = "serialno";
                    getFastbootInfo.RunWorkerAsync();
                }
                else
                {
                    MessageBox.Show(
                        @"A phone in Fastboot mode has not been recognized by the toolkit! Please click the Reload button to check again!",
                        @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void getIMEIButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (statusLabelChange.Text == @"Fastboot")
                {
                    loadingSpinner.Visible = true;
                    getIMEIButton.Enabled = false;
                    AndroidLib.InitialCMD = "getvar";
                    AndroidLib.SecondaryCMD = "imei";
                    getFastbootInfo.RunWorkerAsync();
                }
                else
                {
                    MessageBox.Show(
                        @"A phone in Fastboot mode has not been recognized by the toolkit! Please click the Reload button to check again!",
                        @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void getCIDButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (statusLabelChange.Text == @"Fastboot")
                {
                    loadingSpinner.Visible = true;
                    getCIDButton.Enabled = false;
                    AndroidLib.InitialCMD = "getvar";
                    AndroidLib.SecondaryCMD = "cid";
                    getFastbootInfo.RunWorkerAsync();
                }
                else
                {
                    MessageBox.Show(
                        @"A phone in Fastboot mode has not been recognized by the toolkit! Please click the Reload button to check again!",
                        @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
            MessageBox.Show(
                @"A phone in Fastboot mode has not been recognized by the toolkit! Please click the Reload button to check again!",
                @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void getMIDButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (statusLabelChange.Text == @"Fastboot")
                {
                    loadingSpinner.Visible = true;
                    getMIDButton.Enabled = false;
                    AndroidLib.InitialCMD = "getvar";
                    AndroidLib.SecondaryCMD = "mid";
                    getFastbootInfo.RunWorkerAsync();
                }
                else
                {
                    MessageBox.Show(
                        @"A phone in Fastboot mode has not been recognized by the toolkit! Please click the Reload button to check again!",
                        @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void relockBootloaderButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (statusLabelChange.Text == @"Fastboot")
                {
                    loadingSpinner.Visible = true;
                    getMIDButton.Enabled = false;
                    AndroidLib.InitialCMD = "oem";
                    AndroidLib.SecondaryCMD = "lock";
                    noReturnFastbootCommand.RunWorkerAsync();
                }
                else
                {
                    MessageBox.Show(
                        @"A phone in Fastboot mode has not been recognized by the toolkit! Please click the Reload button to check again!",
                        @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.F5))
            {
                refreshButton.Enabled = false;
                refreshSpinner.Visible = true;
                deviceRecognition.RunWorkerAsync();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void temporaryRecoveryButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (statusLabelChange.Text == @"Fastboot")
                {
                    var dialogResult =
                        MessageBox.Show(
                            @"This will allow you to temporarily flash and use a custom recovery without permanently flashing it. It will default to your previous recovery upon reboot. Are you ready to continue?",
                            @"Temporary Custom Recovery Flash", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (dialogResult == DialogResult.Yes)
                    {
                        openFileDialog1.InitialDirectory = @"C:\";
                        openFileDialog1.Title = @"Please select a recovery .img file";
                        openFileDialog1.FileName = "Choose File...";
                        openFileDialog1.CheckFileExists = true;
                        openFileDialog1.CheckPathExists = true;
                        openFileDialog1.Filter = @" .IMG|*.img";
                        if (openFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            loadingSpinner.Visible = true;
                            temporaryRecoveryButton.Enabled = false;
                            AndroidLib.InitialCMD = "boot";
                            AndroidLib.SecondaryCMD = openFileDialog1.FileName;
                            AndroidLib.Selector = "flashTempRecovery";
                            noReturnFastbootCommand.RunWorkerAsync();
                        }
                    }
                }
                else if (statusLabelChange.Text == @"Online")
                {
                    var dialogResult =
                        MessageBox.Show(
                            @"This will allow you to temporarily flash and use a custom recovery without permanently flashing it. It will default to your previous recovery upon reboot. Are you ready to continue?",
                            @"Temporary Custom Recovery Flash", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (dialogResult == DialogResult.Yes)
                    {
                        openFileDialog1.InitialDirectory = @"C:\";
                        openFileDialog1.Title = @"Please select a recovery .img file";
                        openFileDialog1.FileName = "Choose File...";
                        openFileDialog1.CheckFileExists = true;
                        openFileDialog1.CheckPathExists = true;
                        openFileDialog1.Filter = @" .IMG|*.img";
                        if (openFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            loadingSpinner.Visible = true;
                            temporaryRecoveryButton.Enabled = false;
                            AndroidLib.InitialCMD = "reboot bootloader";
                            AndroidLib.Selector = "flashTempRecovery";
                            noReturnADBCommand.RunWorkerAsync();
                        }
                    }
                }
                else
                {
                    MessageBox.Show(
                        @"A phone has not been recognized by the toolkit! Please click the Reload button to check again!",
                        @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void temporaryKernelButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (statusLabelChange.Text == @"Fastboot")
                {
                    var dialogResult =
                        MessageBox.Show(
                            @"This will allow you to temporarily boot with a custom kernel .img file without permanently flashing it. It will default to your previous kernel upon reboot. Are you ready to continue?",
                            @"Temporary Custom Kernel Flash", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (dialogResult == DialogResult.Yes)
                    {
                        openFileDialog1.InitialDirectory = @"C:\";
                        openFileDialog1.Title = @"Please select a kernel .img file";
                        openFileDialog1.FileName = "Choose File...";
                        openFileDialog1.CheckFileExists = true;
                        openFileDialog1.CheckPathExists = true;
                        openFileDialog1.Filter = @" .IMG|*.img";
                        if (openFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            loadingSpinner.Visible = true;
                            temporaryKernelButton.Enabled = false;
                            AndroidLib.InitialCMD = "boot";
                            AndroidLib.SecondaryCMD = openFileDialog1.FileName;
                            AndroidLib.Selector = "flashTempKernel";
                            noReturnFastbootCommand.RunWorkerAsync();
                        }
                    }
                }
                else if (statusLabelChange.Text == @"Online")
                {
                    var dialogResult =
                        MessageBox.Show(
                            @"This will allow you to temporarily boot with a custom kernel .img file without permanently flashing it. It will default to your previous kernel upon reboot. Are you ready to continue?",
                            @"Temporary Custom Kernel Flash", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (dialogResult == DialogResult.Yes)
                    {
                        openFileDialog1.InitialDirectory = @"C:\";
                        openFileDialog1.Title = @"Please select a kernel .img file";
                        openFileDialog1.FileName = "Choose File...";
                        openFileDialog1.CheckFileExists = true;
                        openFileDialog1.CheckPathExists = true;
                        openFileDialog1.Filter = @" .IMG|*.img";
                        if (openFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            loadingSpinner.Visible = true;
                            temporaryKernelButton.Enabled = false;
                            AndroidLib.InitialCMD = "reboot bootloader";
                            AndroidLib.Selector = "flashTempKernel";
                            noReturnADBCommand.RunWorkerAsync();
                        }
                    }
                }
                else
                {
                    MessageBox.Show(
                        @"A phone has not been recognized by the toolkit! Please click the Reload button to check again!",
                        @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void writeSuperCIDButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (statusLabelChange.Text == @"Fastboot")
                {
                    var cid = Interaction.InputBox("Please input what you would like your CID to be. This function only works if you currently have S-OFF on your phone. Otherwise, it does nothing.", "SuperCID", "", 775, 450);
                    loadingSpinner.Visible = true;
                    writeSuperCIDButton.Enabled = false;
                    AndroidLib.InitialCMD = "oem";
                    AndroidLib.SecondaryCMD = "writecid " + cid;
                    AndroidLib.Selector = "writeSuperCID";
                    noReturnFastbootCommand.RunWorkerAsync();
                }
                else
                {
                    MessageBox.Show(
                        @"A phone has not been recognized by the toolkit! Please click the Reload button to check again!",
                        @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void writeIMEIButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (statusLabelChange.Text == @"Fastboot")
                {
                    var imei = Interaction.InputBox("Please input what you would like your IMEI to be. This function only works if you currently have S-OFF on your phone. Otherwise, it does nothing.", "SuperCID", "", 775, 450);
                    loadingSpinner.Visible = true;
                    writeIMEIButton.Enabled = false;
                    AndroidLib.InitialCMD = "oem";
                    AndroidLib.SecondaryCMD = "writeimei " + imei;
                    AndroidLib.Selector = "writeIMEI";
                    noReturnFastbootCommand.RunWorkerAsync();
                }
                else
                {
                    MessageBox.Show(
                        @"A phone has not been recognized by the toolkit! Please click the Reload button to check again!",
                        @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void donateTile_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("http://forum.xda-developers.com/donatetome.php?u=4485224");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void twitterTile_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("http://twitter.com/WindyCityRockr");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void pmTile_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("http://forum.xda-developers.com/private.php?do=newpm&u=4485224");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void toolkitThreadButton_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("http://forum.xda-developers.com/showthread.php?t=2430358");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void instructionsButton_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("http://forum.xda-developers.com/showpost.php?p=45362597&postcount=2");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }

        private void phoneForumButton_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("http://forum.xda-developers.com/htc-one-x2");
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    @"An error has occured! A log file has been placed in the Logs folder within the Data folder. Please send the file to WindyCityRockr or post the file in the toolkit thread.",
                    @"Houston, we have a problem!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var fileDateTime = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("HHmmss");
                var file = new StreamWriter("./Data/Logs/" + fileDateTime + ".txt");
                file.WriteLine(ex);
                file.Close();
            }
        }
    }
}