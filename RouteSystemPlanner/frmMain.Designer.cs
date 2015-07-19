namespace CountyRoutePlanner
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblOrigin = new System.Windows.Forms.Label();
            this.txtOrigin = new System.Windows.Forms.TextBox();
            this.lblDestination = new System.Windows.Forms.Label();
            this.txtDestination = new System.Windows.Forms.TextBox();
            this.txtMapsURL = new System.Windows.Forms.TextBox();
            this.lblMapsURL = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnPlanIt = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblOrigin
            // 
            this.lblOrigin.AutoSize = true;
            this.lblOrigin.Location = new System.Drawing.Point(43, 15);
            this.lblOrigin.Name = "lblOrigin";
            this.lblOrigin.Size = new System.Drawing.Size(34, 13);
            this.lblOrigin.TabIndex = 0;
            this.lblOrigin.Text = "Origin";
            // 
            // txtOrigin
            // 
            this.txtOrigin.Location = new System.Drawing.Point(83, 12);
            this.txtOrigin.Name = "txtOrigin";
            this.txtOrigin.Size = new System.Drawing.Size(163, 20);
            this.txtOrigin.TabIndex = 1;
            this.txtOrigin.TextChanged += new System.EventHandler(this.txtOrigin_TextChanged);
            // 
            // lblDestination
            // 
            this.lblDestination.AutoSize = true;
            this.lblDestination.Location = new System.Drawing.Point(17, 41);
            this.lblDestination.Name = "lblDestination";
            this.lblDestination.Size = new System.Drawing.Size(60, 13);
            this.lblDestination.TabIndex = 2;
            this.lblDestination.Text = "Destination";
            // 
            // txtDestination
            // 
            this.txtDestination.Location = new System.Drawing.Point(83, 38);
            this.txtDestination.Name = "txtDestination";
            this.txtDestination.Size = new System.Drawing.Size(163, 20);
            this.txtDestination.TabIndex = 3;
            this.txtDestination.TextChanged += new System.EventHandler(this.txtDestination_TextChanged);
            // 
            // txtMapsURL
            // 
            this.txtMapsURL.Location = new System.Drawing.Point(83, 92);
            this.txtMapsURL.Name = "txtMapsURL";
            this.txtMapsURL.Size = new System.Drawing.Size(307, 20);
            this.txtMapsURL.TabIndex = 4;
            this.txtMapsURL.TextChanged += new System.EventHandler(this.txtMapsURL_TextChanged);
            // 
            // lblMapsURL
            // 
            this.lblMapsURL.AutoSize = true;
            this.lblMapsURL.Location = new System.Drawing.Point(19, 95);
            this.lblMapsURL.Name = "lblMapsURL";
            this.lblMapsURL.Size = new System.Drawing.Size(58, 13);
            this.lblMapsURL.TabIndex = 5;
            this.lblMapsURL.Text = "Maps URL";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(41, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "OR";
            // 
            // btnPlanIt
            // 
            this.btnPlanIt.Location = new System.Drawing.Point(83, 131);
            this.btnPlanIt.Name = "btnPlanIt";
            this.btnPlanIt.Size = new System.Drawing.Size(75, 23);
            this.btnPlanIt.TabIndex = 7;
            this.btnPlanIt.Text = "Plan It!";
            this.btnPlanIt.UseVisualStyleBackColor = true;
            this.btnPlanIt.Click += new System.EventHandler(this.btnPlanIt_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(83, 174);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutput.Size = new System.Drawing.Size(307, 352);
            this.txtOutput.TabIndex = 8;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 538);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.btnPlanIt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblMapsURL);
            this.Controls.Add(this.txtMapsURL);
            this.Controls.Add(this.txtDestination);
            this.Controls.Add(this.lblDestination);
            this.Controls.Add(this.txtOrigin);
            this.Controls.Add(this.lblOrigin);
            this.Name = "frmMain";
            this.Text = "County Route Planner";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblOrigin;
        private System.Windows.Forms.TextBox txtOrigin;
        private System.Windows.Forms.Label lblDestination;
        private System.Windows.Forms.TextBox txtDestination;
        private System.Windows.Forms.TextBox txtMapsURL;
        private System.Windows.Forms.Label lblMapsURL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnPlanIt;
        private System.Windows.Forms.TextBox txtOutput;
    }
}

