using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Runtime.InteropServices;

// This is the code for your desktop appSystem.Runtime.Serialization.Formatters.Binary
// Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.



namespace Project__Making_Life_Easier
{

    public partial class Form1 : Form
    {


        //List<LogItem> shelfTable = new List<LogItem>();  // OLD Script, was working with JSON before

        public Form1()
        {
            InitializeComponent();

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult result = MessageBox.Show("Do you want to save?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    UpdateTable("Easy");
                    UpdateTable("Function");
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }

            }
            base.OnFormClosing(e);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        /*   var shelf = "C:\\Database - Make Life Easier\\TableOfRowsBeyond.json";

            if (File.Exists(shelf))
            { shelfTable = JsonConvert.DeserializeObject<List<LogItem>>(File.ReadAllText(shelf)); }*/

            UpdateTable("Easy");
            UpdateTable("Function");

          /*  defaultSize.Width = this.Size.Width;
            defaultSize.Height = this.Size.Height;*/
        }

        private bool isNotEmpty(string str)
        {

            if (!String.IsNullOrEmpty(str))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


       /* private void editShelf(string todo, string shelf, string row)
        {
            int i = -1; foreach (var item in shelfTable) { i = ++i; if (item.Name == shelf) { break; } }
            if (i >= 0 && i < shelfTable.Count) // This shelf exists
            {

                if (todo == "Add")
                {
                    if (!shelfTable[i].Rows.Contains(row))
                    { shelfTable[i].Rows.Add(row); }
                }
                else if (todo == "Remove")
                {
                    if (shelfTable[i].Rows.Contains(row))
                    { shelfTable[i].Rows.Remove(row); }
                }
            }
            else
            {
                LogItem Entry = new LogItem { Name = shelf, Rows = new List<string> { row } };

                shelfTable.Add(Entry); // Table
            }
        }*/

        public void UpdateTable(string menu)
        {
            if (menu == "Easy")
            {
             /*   File.WriteAllText("C:\\Database - Make Life Easier\\TableOfRowsBeyond.json", JsonConvert.SerializeObject(shelfTable));*/
                tableEasyTableAdapter.Fill(databaseEasyDataSet.TableEasy);
            }
            else if (menu == "Function")
            {
                tableFunctionTableAdapter.Fill(functionsDataSet.TableFunction);
            }
        }

        private void SetCellWithFocus(int row, int menu)
        {  // Set the current cell to cell (>) , row (row (v) )
            if (menu == 1)
            { dataGridView1.CurrentCell = dataGridView1[0, row]; }
            else if (menu == 2)
            { dataGridView2.CurrentCell = dataGridView2[0, row]; }
        }

        private void ButtonSearch_Click(object sender, EventArgs e) // Easier: Search
        {

            if (isNotEmpty(textBox0.Text))
            {
                var reff = textBox0.Text;
                if (tableEasyTableAdapter.ByRef(databaseEasyDataSet.TableEasy, reff) == 0)
                {
                    MessageBox.Show(" Reference (" + reff + ") is not found in the system"); UpdateTable("Easy");
                }

            }
            else if (isNotEmpty(textBox4.Text))
            {
                if (isNotEmpty(textBox5.Text))
                {
                    tableEasyTableAdapter.ByShelfAndRow(databaseEasyDataSet.TableEasy, textBox4.Text, textBox5.Text);
                }
                else
                {
                    tableEasyTableAdapter.ByShelf(databaseEasyDataSet.TableEasy, textBox4.Text);
                }
            }
            else
            {
                MessageBox.Show("There is nothing to search (Ref nor Name nor Shelf)"); UpdateTable("Easy");
            }
        }

        private void Button1_Click(object sender, EventArgs e) // Easier: Update
        {
            UpdateTable("Easy");
        }

        private void Button4_Click(object sender, EventArgs e) // Easier: Save
        {
            UpdateTable("Easy");
            MessageBox.Show("All entries has been saved");
        }

        private void Button6_Click(object sender, EventArgs e) // Easier: Clear
        {
            dataGridView1.ClearSelection(); dataGridView1.CancelEdit();
            textBox0.Text = ""; textBox4.Text = ""; textBox5.Text = "";

        }
        private void Button5_Click(object sender, EventArgs e) // Easier: Add
        {
            var str0 = textBox0.Text; var str4 = textBox4.Text; ; var str5 = textBox5.Text;
            if (isNotEmpty(str0) && isNotEmpty(str4) && isNotEmpty(str5))
            {
                DialogResult result = MessageBox.Show("Do you add new entry? {" + str0 + ", " + str4 + ", " + str5 + "}", "Do you Confirm?", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    if (tableEasyTableAdapter.RowExists(databaseEasyDataSet.TableEasy, str0, str4, str5) == 0)
                    {
                        tableEasyBindingSource.AddNew();
                        textBox0.Text = str0; textBox4.Text = str4; textBox5.Text = str5;
                        tableEasyTableAdapter.Insert(str0, str4, str5);
                        tableEasyBindingSource.EndEdit();
                      /*  editShelf("Add", str4, str5);*/

                        UpdateTable("Easy");
                        databaseEasyDataSet.AcceptChanges();
                        SetCellWithFocus(dataGridView1.RowCount - 1, 1);
                    }
                    else
                    {
                        MessageBox.Show("There already exists data like this");
                    }
                }
            }
            else
            {
                MessageBox.Show("Data is missing");
            }
        }

        private void Button3_Click(object sender, EventArgs e) // Easier : Edit -- First is Edit and the second is Confirm
        {
            var str0 = textBox0.Text; var str4 = textBox4.Text; ; var str5 = textBox5.Text;
            var currentCell = dataGridView1.CurrentCell.RowIndex;

            if (isNotEmpty(str0) && isNotEmpty(str4) && isNotEmpty(str5))
            {
                DialogResult result = MessageBox.Show("Do you edit this entry? {" + str0 + ", " + str4 + ", " + str5 + "} ", "ONLY USE WHEN YOU ARENT SEARCHING", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    var z = databaseEasyDataSet.TableEasy.Rows.Count;
                    try
                    {

                        tableEasyTableAdapter.Insert(str0, str4, str5);

                        UpdateTable("Easy");
                        SetCellWithFocus(0, 1);
                        SetCellWithFocus(currentCell, 1);
                        var n0 = textBox0.Text; var n4 = textBox4.Text; ; var n5 = textBox5.Text;


                        tableEasyTableAdapter.Delete(n0, n4, n5);

                        if (n0 == str0 && n4 == str4 && n5 == str5)
                        {
                            tableEasyTableAdapter.Insert(str0, str4, str5);
                        }

                        UpdateTable("Easy");

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Select data from the table first before proceeding");
            }
        }

        private void Button7_Click(object sender, EventArgs e) // Easier: Remove
        {
            if (dataGridView1.CurrentCell != null)
            {
                var currentCellx = dataGridView1.CurrentCell.ColumnIndex; var currentCelly = dataGridView1.CurrentCell.RowIndex;
                var str0 = textBox0.Text; var str4 = textBox4.Text; ; var str5 = textBox5.Text;
                DialogResult result = MessageBox.Show("Do you remove this entry? {" + str0 + ", " + str4 + ", " + str5 + "} ", "Do you Confirm?", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        //editShelf("Remove", str4, str5);
                        tableEasyTableAdapter.Delete(str0, str4, str5);
                        tableEasyBindingSource.EndEdit();
                        databaseEasyDataSet.AcceptChanges();

                        UpdateTable("Easy");
                        if (dataGridView1.RowCount > 0 && currentCelly > 0)
                        {
                            dataGridView1.CurrentCell = dataGridView1[currentCellx, currentCelly - 1];
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Select data first before proceeding");
            }
        }

        private void Button11_Click(object sender, EventArgs e) // Function: Update
        {
            UpdateTable("Function");
        }

        private void Button10_Click(object sender, EventArgs e) // Function: Save
        {
            UpdateTable("Function");
            MessageBox.Show("All entries has been saved");
        }

        private void Button12_Click(object sender, EventArgs e) // Function: Clear
        {
            textBox7.Text = ""; richTextBox2.Text = "";
        }

        private void Button2_Click(object sender, EventArgs e) // Function: Add
        {
            var str1 = textBox7.Text; var str2 = richTextBox2.Text;
            if (isNotEmpty(str1) && isNotEmpty(str2))
            {
                if (tableFunctionTableAdapter.ByName(functionsDataSet.TableFunction, str1) == 0)
                {
                    DialogResult result = MessageBox.Show("Do you add new entry? ('" + str1 + "') ", "Do you Confirm?", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        tableFunctionBindingSource.AddNew();
                        textBox7.Text = str1; richTextBox2.Text = str2;
                        tableFunctionTableAdapter.Insert(str1, str2);
                        tableFunctionBindingSource.EndEdit(); UpdateTable("Function");
                        functionsDataSet.AcceptChanges();
                        SetCellWithFocus(dataGridView2.RowCount - 1, 2);
                    }

                }
                else
                {
                    MessageBox.Show("There already exists data with the name ('" + str1 + "')");
                }
            }
            else
            {
                MessageBox.Show("Data is missing");
            }

        }

        private void Button8_Click(object sender, EventArgs e) // Function: Search
        {
            var str = textBox7.Text;
            if (isNotEmpty(str))
            {
                if (tableFunctionTableAdapter.ByName(functionsDataSet.TableFunction, str) == 1)
                {
                    MessageBox.Show("Found");
                }
                else
                {
                    MessageBox.Show("'" + str + "' is not found in the system"); UpdateTable("Function");
                }
            }
            else
            {
                MessageBox.Show("There is nothing to search (Name)"); UpdateTable("Function");
            }
        }

        private void Button9_Click(object sender, EventArgs e) // Function: Remove
        {
            if (dataGridView2.CurrentCell != null)
            {
                var currentCellx = dataGridView2.CurrentCell.ColumnIndex; var currentCelly = dataGridView2.CurrentCell.RowIndex;
                DialogResult result = MessageBox.Show("Do you remove this entry? ('" + textBox7.Text + "')", "Do you Confirm?", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        tableFunctionTableAdapter.Delete(textBox7.Text);
                        tableFunctionBindingSource.EndEdit();
                        functionsDataSet.AcceptChanges();

                        UpdateTable("Function");
                        if (dataGridView1.RowCount > 0 && currentCelly > 0)
                        {
                            dataGridView2.CurrentCell = dataGridView2[currentCellx, currentCelly - 1];
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Select data first before proceeding");
            }
        }

        private void TextUpper(object sender, EventArgs e)
        {
            textBox0.Text = textBox0.Text.ToUpper();
            textBox4.Text = textBox4.Text.ToUpper();
            textBox5.Text = textBox5.Text.ToUpper();
        }

        private void TextBox4_TextUpdate(object sender, EventArgs e)
        {
            textBox5.Text = "";
            textBox5.Items.Clear();
            if (textBox4.Text == "")
            {
                textBox5.Enabled = false;
            }
            else
            {
                textBox5.Enabled = true;
/*                var i = -1; bool f = false; foreach (var item in shelfTable) { i = ++i; if (item.Name == textBox4.Text) { f = true; break; } }
                if (f) {
                    if (i >= 0 && i < shelfTable.Count)
                    {
                        foreach (var item in shelfTable[i].Rows)
                        {
                            textBox5.Items.Add(item);
                        }
                    }
                }*/
            }


        }

        private void button20_Click(object sender, EventArgs e) // Function; Add
        {

            if (dataGridView2.CurrentCell != null)
            {
                var currentCellx = dataGridView2.CurrentCell.ColumnIndex; var currentCelly = dataGridView2.CurrentCell.RowIndex;
                var str1 = textBox7.Text; var str2 = richTextBox2.Text;
                DialogResult result = MessageBox.Show("Do you edit this entry? {" + str1 + "} ", "ONLY USE WHEN YOU ARENT SEARCHING TO AVOID BUGS", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        tableFunctionBindingSource.AddNew();
                        textBox7.Text = str1; richTextBox2.Text = str2;
                        tableFunctionTableAdapter.Insert(str1, str2);
                        tableFunctionBindingSource.EndEdit();

                        UpdateTable("Function");
                        functionsDataSet.AcceptChanges();
                        SetCellWithFocus(dataGridView2.RowCount - 1, 2);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Select data first before proceeding");
                }
            }

        }


        private void shelf_button_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            tableEasyTableAdapter.ByShelfAndRow(databaseEasyDataSet.TableEasy, (String)btn.Tag, btn.Text);
            dd.SelectedTab = tabPage1;
        }

        private void buttonLittleS_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            tableEasyTableAdapter.ByShelfAndRow(databaseEasyDataSet.TableEasy, "LS", btn.Text);
            dd.SelectedTab = tabPage1;
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void tab_dd_Click(object sender, EventArgs e)  {
            //this.Size = new System.Drawing.Size(defaultSize.Width, defaultSize.Height);
           //this.WindowState = FormWindowState.Maximized;
        }

        private void Form1_Layout(object sender, LayoutEventArgs e)
        {

        }
    }
}



