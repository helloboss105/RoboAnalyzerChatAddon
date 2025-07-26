namespace RoboAnalyzerChatAddon.Forms
{
    partial class LobbyForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnCreateRoom;
        private System.Windows.Forms.Button btnJoinRoom;
        private System.Windows.Forms.TextBox txtRoomCode;
        private System.Windows.Forms.Label lblRoomCode;
        private System.Windows.Forms.Label lblTitle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.btnCreateRoom = new System.Windows.Forms.Button();
            this.btnJoinRoom = new System.Windows.Forms.Button();
            this.txtRoomCode = new System.Windows.Forms.TextBox();
            this.lblRoomCode = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(75, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(200, 24);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "RoboAnalyzer Chat";

            // btnCreateRoom
            this.btnCreateRoom.Location = new System.Drawing.Point(100, 70);
            this.btnCreateRoom.Name = "btnCreateRoom";
            this.btnCreateRoom.Size = new System.Drawing.Size(150, 35);
            this.btnCreateRoom.TabIndex = 1;
            this.btnCreateRoom.Text = "Create New Room";
            this.btnCreateRoom.UseVisualStyleBackColor = true;
            this.btnCreateRoom.Click += new System.EventHandler(this.btnCreateRoom_Click);

            // lblRoomCode
            this.lblRoomCode.AutoSize = true;
            this.lblRoomCode.Location = new System.Drawing.Point(30, 135);
            this.lblRoomCode.Name = "lblRoomCode";
            this.lblRoomCode.Size = new System.Drawing.Size(67, 13);
            this.lblRoomCode.TabIndex = 2;
            this.lblRoomCode.Text = "Room Code:";

            // txtRoomCode
            this.txtRoomCode.Location = new System.Drawing.Point(103, 132);
            this.txtRoomCode.MaxLength = 6;
            this.txtRoomCode.Name = "txtRoomCode";
            this.txtRoomCode.Size = new System.Drawing.Size(100, 20);
            this.txtRoomCode.TabIndex = 3;
            this.txtRoomCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtRoomCode_KeyPress);

            // btnJoinRoom
            this.btnJoinRoom.Location = new System.Drawing.Point(220, 130);
            this.btnJoinRoom.Name = "btnJoinRoom";
            this.btnJoinRoom.Size = new System.Drawing.Size(75, 23);
            this.btnJoinRoom.TabIndex = 4;
            this.btnJoinRoom.Text = "Join Room";
            this.btnJoinRoom.UseVisualStyleBackColor = true;
            this.btnJoinRoom.Click += new System.EventHandler(this.btnJoinRoom_Click);

            // LobbyForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 200);
            this.Controls.Add(this.btnJoinRoom);
            this.Controls.Add(this.txtRoomCode);
            this.Controls.Add(this.lblRoomCode);
            this.Controls.Add(this.btnCreateRoom);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "LobbyForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RoboAnalyzer Chat Lobby";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}