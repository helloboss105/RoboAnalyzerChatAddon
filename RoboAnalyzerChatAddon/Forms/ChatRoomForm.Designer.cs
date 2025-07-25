namespace RoboAnalyzerChatAddon.Forms
{
    partial class ChatRoomForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.RichTextBox rtbChat;
        private System.Windows.Forms.ListBox lstUsers;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnShareFile;
        private System.Windows.Forms.Label lblRoomCode;
        private System.Windows.Forms.Label lblUsers;

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
            this.rtbChat = new System.Windows.Forms.RichTextBox();
            this.lstUsers = new System.Windows.Forms.ListBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnShareFile = new System.Windows.Forms.Button();
            this.lblRoomCode = new System.Windows.Forms.Label();
            this.lblUsers = new System.Windows.Forms.Label();
            this.SuspendLayout();

            // lblRoomCode
            this.lblRoomCode.AutoSize = true;
            this.lblRoomCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblRoomCode.Location = new System.Drawing.Point(12, 9);
            this.lblRoomCode.Name = "lblRoomCode";
            this.lblRoomCode.Size = new System.Drawing.Size(89, 17);
            this.lblRoomCode.TabIndex = 0;
            this.lblRoomCode.Text = "Room Code:";

            // rtbChat
            this.rtbChat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbChat.Location = new System.Drawing.Point(12, 35);
            this.rtbChat.Name = "rtbChat";
            this.rtbChat.ReadOnly = true;
            this.rtbChat.Size = new System.Drawing.Size(476, 320);
            this.rtbChat.TabIndex = 1;
            this.rtbChat.Text = "";

            // lblUsers
            this.lblUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUsers.AutoSize = true;
            this.lblUsers.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblUsers.Location = new System.Drawing.Point(494, 35);
            this.lblUsers.Name = "lblUsers";
            this.lblUsers.Size = new System.Drawing.Size(44, 15);
            this.lblUsers.TabIndex = 2;
            this.lblUsers.Text = "Users:";

            // lstUsers
            this.lstUsers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstUsers.FormattingEnabled = true;
            this.lstUsers.Location = new System.Drawing.Point(494, 53);
            this.lstUsers.Name = "lstUsers";
            this.lstUsers.Size = new System.Drawing.Size(120, 302);
            this.lstUsers.TabIndex = 3;

            // txtMessage
            this.txtMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMessage.Location = new System.Drawing.Point(12, 365);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(350, 40);
            this.txtMessage.TabIndex = 4;
            this.txtMessage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtMessage_KeyPress);

            // btnSend
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Location = new System.Drawing.Point(368, 365);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(60, 40);
            this.btnSend.TabIndex = 5;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);

            // btnShareFile
            this.btnShareFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShareFile.Location = new System.Drawing.Point(434, 365);
            this.btnShareFile.Name = "btnShareFile";
            this.btnShareFile.Size = new System.Drawing.Size(75, 40);
            this.btnShareFile.TabIndex = 6;
            this.btnShareFile.Text = "Share File";
            this.btnShareFile.UseVisualStyleBackColor = true;
            this.btnShareFile.Click += new System.EventHandler(this.btnShareFile_Click);

            // ChatRoomForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 417);
            this.Controls.Add(this.btnShareFile);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.lstUsers);
            this.Controls.Add(this.lblUsers);
            this.Controls.Add(this.rtbChat);
            this.Controls.Add(this.lblRoomCode);
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "ChatRoomForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chat Room";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}