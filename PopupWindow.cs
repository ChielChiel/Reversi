using System;
using System.Drawing;
using System.Windows.Forms;

public partial class PopupWindow : Form
{
    public bool isPerson;
    private TextBox playerOneTextBox;
    private TextBox playerTwoTextBox;
    private TextBox gameBoardTextBox;

    public String namePlayerOne;
    public String namePlayerTwo;
    public int gameBoardSize;

    public PopupWindow(bool isPerson)
    {
        this.isPerson = isPerson;
        this.ControlBox = false;
        this.Size = new Size(250, 300);
        this.Text = "Stel game in";

        //Player one textbox
        this.playerOneTextBox = new TextBox();
        this.playerOneTextBox.Size = new Size(200, 100);
        this.playerOneTextBox.Location = new Point(20, 40);
        this.playerOneTextBox.Name = "playerOne";
        this.Controls.Add(this.playerOneTextBox);

        Label playerOneLabel = new Label();
        playerOneLabel.Text = "Naam speler 1";
        playerOneLabel.Location = new Point(20, 20);
        playerOneLabel.Name = "labelPlayerOne";
        this.Controls.Add(playerOneLabel);

        //Player two textbox
        if (isPerson)
        {
            Label playerTwoLabel = new Label();
            playerTwoLabel.Text = "Naam speler 2";
            playerTwoLabel.Location = new Point(20, 75);
            playerTwoLabel.Name = "labelPlayerTwo";
            this.Controls.Add(playerTwoLabel);

            this.playerTwoTextBox = new TextBox();
            this.playerTwoTextBox.Size = new Size(200, 100);
            this.playerTwoTextBox.Location = new Point(20, 100);
            this.playerTwoTextBox.Name = "playerTwo";
            this.Controls.Add(this.playerTwoTextBox);
        }
        //boardsize
        Label boardSizeLabel = new Label();
        boardSizeLabel.Text = "Afmeting van het spelbord";
        boardSizeLabel.Location = new Point(20, 130);
        boardSizeLabel.Size = new Size(200, 20);
        boardSizeLabel.Name = "labelBoardSize";
        this.Controls.Add(boardSizeLabel);

        this.gameBoardTextBox = new TextBox();
        this.gameBoardTextBox.Size = new Size(50, 100);
        this.gameBoardTextBox.Location = new Point(20, 150);
        this.gameBoardTextBox.Name = "gameBoardTextBox";
        this.Controls.Add(this.gameBoardTextBox);



        //Cancel button
        Button cancelButton = new Button();
        cancelButton.Size = new Size(80, 50);
        cancelButton.Location = new Point(20, 180);
        cancelButton.Name = "cancelButton";
        cancelButton.Text = "Annuleer";
        cancelButton.Click += this.cancelClick;
        this.CancelButton = cancelButton;

        this.Controls.Add(cancelButton);

        //Submit button
        Button submitButton = new Button();
        submitButton.Size = new Size(80, 50);
        submitButton.Location = new Point(140, 180);
        submitButton.Name = "submitButton";
        submitButton.Text = "Start game";
        submitButton.Click += this.submitClick;
        this.AcceptButton = submitButton;

        this.Controls.Add(submitButton);



    }

    private void submitClick(object o, EventArgs e)
    {
        bool allOk = true;
        string playerOne = this.playerOneTextBox.Text;
        string playerTwo = (this.isPerson ? this.playerTwoTextBox.Text : "Bot Frank");
        int BoardSize;
        if (int.TryParse(this.gameBoardTextBox.Text, out BoardSize))
        {
            //parsing successful 

            if (BoardSize < 3 && BoardSize > 10)
            {
                //Board heeft juiste afmetingen 
                allOk = false;
            }
        }
        else
        {
            //parsing failed.
            allOk = false;
        }

        if (String.IsNullOrEmpty(this.playerOneTextBox.Text))
        {
            allOk = false;
        }

        if (this.isPerson && String.IsNullOrEmpty(this.playerOneTextBox.Text))
        {
            allOk = false;
        }


        if (allOk)
        {
            this.namePlayerOne = playerOne;
            this.namePlayerTwo = playerTwo;
            this.gameBoardSize = BoardSize;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        else
        {
            Console.WriteLine("Fix je inputs");
        }


    }

    private void cancelClick(object o, EventArgs e)
    {
        this.DialogResult = DialogResult.Cancel;
        this.Close();
    }


}
