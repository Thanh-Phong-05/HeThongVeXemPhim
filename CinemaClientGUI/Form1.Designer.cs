namespace CinemaClientGUI
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.GroupBox groupMovies;
        private System.Windows.Forms.GroupBox groupShows;
        private System.Windows.Forms.GroupBox groupSeats;
        private System.Windows.Forms.GroupBox groupActions;

        private System.Windows.Forms.DataGridView dataGridMovies;
        private System.Windows.Forms.DataGridView dataGridShows;
        private System.Windows.Forms.DataGridView dataGridSeats;
        private System.Windows.Forms.Panel panelSeats;

        private System.Windows.Forms.TextBox txtMovieId;
        private System.Windows.Forms.TextBox txtShowId;
        private System.Windows.Forms.TextBox txtSeats;

        private System.Windows.Forms.Label lblMovieId;
        private System.Windows.Forms.Label lblShowId;
        private System.Windows.Forms.Label lblSeats;

        private System.Windows.Forms.Button btnListMovies;
        private System.Windows.Forms.Button btnListShows;
        private System.Windows.Forms.Button btnViewSeats;
        private System.Windows.Forms.Button btnBook;
        private System.Windows.Forms.Button btnRelease;

        private System.Windows.Forms.FlowLayoutPanel flowButtons;

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
            this.groupMovies = new System.Windows.Forms.GroupBox();
            this.groupShows = new System.Windows.Forms.GroupBox();
            this.groupSeats = new System.Windows.Forms.GroupBox();
            this.groupActions = new System.Windows.Forms.GroupBox();

            this.dataGridMovies = new System.Windows.Forms.DataGridView();
            this.dataGridShows = new System.Windows.Forms.DataGridView();
            this.dataGridSeats = new System.Windows.Forms.DataGridView();
            this.panelSeats = new System.Windows.Forms.Panel();

            this.txtMovieId = new System.Windows.Forms.TextBox();
            this.txtShowId = new System.Windows.Forms.TextBox();
            this.txtSeats = new System.Windows.Forms.TextBox();

            this.lblMovieId = new System.Windows.Forms.Label();
            this.lblShowId = new System.Windows.Forms.Label();
            this.lblSeats = new System.Windows.Forms.Label();

            this.btnListMovies = new System.Windows.Forms.Button();
            this.btnListShows = new System.Windows.Forms.Button();
            this.btnViewSeats = new System.Windows.Forms.Button();
            this.btnBook = new System.Windows.Forms.Button();
            this.btnRelease = new System.Windows.Forms.Button();

            this.flowButtons = new System.Windows.Forms.FlowLayoutPanel();

            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(950, 700);
            this.Name = "Form1";
            this.Text = "🎬 Cinema Client";
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.Load += new System.EventHandler(this.Form1_Load);

            // === GroupMovies ===
            this.groupMovies.Text = "🎬 Movies";
            this.groupMovies.Location = new System.Drawing.Point(20, 20);
            this.groupMovies.Size = new System.Drawing.Size(500, 200);

            this.dataGridMovies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridMovies.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridMovies.BackgroundColor = System.Drawing.Color.White;
            this.groupMovies.Controls.Add(this.dataGridMovies);

            // === GroupShows ===
            this.groupShows.Text = "📅 Shows";
            this.groupShows.Location = new System.Drawing.Point(20, 240);
            this.groupShows.Size = new System.Drawing.Size(500, 180);

            this.dataGridShows.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridShows.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridShows.BackgroundColor = System.Drawing.Color.White;
            this.groupShows.Controls.Add(this.dataGridShows);

            // === GroupSeats ===
            this.groupSeats.Text = "💺 Seats";
            this.groupSeats.Location = new System.Drawing.Point(20, 440);
            this.groupSeats.Size = new System.Drawing.Size(500, 220);

            this.dataGridSeats.Location = new System.Drawing.Point(10, 25);
            this.dataGridSeats.Size = new System.Drawing.Size(300, 180);
            this.dataGridSeats.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.groupSeats.Controls.Add(this.dataGridSeats);

            this.panelSeats.Location = new System.Drawing.Point(320, 25);
            this.panelSeats.Size = new System.Drawing.Size(160, 180);
            this.panelSeats.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelSeats.BackColor = System.Drawing.Color.WhiteSmoke;
            this.groupSeats.Controls.Add(this.panelSeats);

            // === GroupActions ===
            this.groupActions.Text = "⚡ Actions";
            this.groupActions.Location = new System.Drawing.Point(550, 20);
            this.groupActions.Size = new System.Drawing.Size(370, 640);

            // Labels + TextBoxes
            this.lblMovieId.Text = "Movie ID:";
            this.lblMovieId.Location = new System.Drawing.Point(20, 40);
            this.txtMovieId.Location = new System.Drawing.Point(120, 40);
            this.txtMovieId.Size = new System.Drawing.Size(200, 25);

            this.lblShowId.Text = "Show ID:";
            this.lblShowId.Location = new System.Drawing.Point(20, 80);
            this.txtShowId.Location = new System.Drawing.Point(120, 80);
            this.txtShowId.Size = new System.Drawing.Size(200, 25);

            this.lblSeats.Text = "Seats (A1,A2,...):";
            this.lblSeats.Location = new System.Drawing.Point(20, 120);
            this.txtSeats.Location = new System.Drawing.Point(160, 120);
            this.txtSeats.Size = new System.Drawing.Size(160, 25);

            // Buttons with FlowLayout
            this.flowButtons.Location = new System.Drawing.Point(20, 180);
            this.flowButtons.Size = new System.Drawing.Size(320, 150);
            this.flowButtons.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flowButtons.WrapContents = true;

            this.btnListMovies.Text = "List Movies";
            this.btnListMovies.Click += new System.EventHandler(this.btnListMovies_Click);

            this.btnListShows.Text = "List Shows";
            this.btnListShows.Click += new System.EventHandler(this.btnListShows_Click);

            this.btnViewSeats.Text = "View Seats";
            this.btnViewSeats.Click += new System.EventHandler(this.btnViewSeats_Click);

            this.btnBook.Text = "Book Seats";
            this.btnBook.Click += new System.EventHandler(this.btnBook_Click);

            this.btnRelease.Text = "Release Seats";
            this.btnRelease.Click += new System.EventHandler(this.btnRelease_Click);

            this.flowButtons.Controls.Add(this.btnListMovies);
            this.flowButtons.Controls.Add(this.btnListShows);
            this.flowButtons.Controls.Add(this.btnViewSeats);
            this.flowButtons.Controls.Add(this.btnBook);
            this.flowButtons.Controls.Add(this.btnRelease);

            this.groupActions.Controls.Add(this.lblMovieId);
            this.groupActions.Controls.Add(this.txtMovieId);
            this.groupActions.Controls.Add(this.lblShowId);
            this.groupActions.Controls.Add(this.txtShowId);
            this.groupActions.Controls.Add(this.lblSeats);
            this.groupActions.Controls.Add(this.txtSeats);
            this.groupActions.Controls.Add(this.flowButtons);

            // === Add to Form ===
            this.Controls.Add(this.groupMovies);
            this.Controls.Add(this.groupShows);
            this.Controls.Add(this.groupSeats);
            this.Controls.Add(this.groupActions);

            this.ResumeLayout(false);
        }
    }
}
