namespace CinemaClientGUI
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TextBox txtMovieId;
        private System.Windows.Forms.TextBox txtShowId;
        private System.Windows.Forms.TextBox txtSeats;

        private System.Windows.Forms.Button btnListMovies;
        private System.Windows.Forms.Button btnListShows;
        private System.Windows.Forms.Button btnViewSeats;
        private System.Windows.Forms.Button btnBook;
        private System.Windows.Forms.Button btnRelease;

        private System.Windows.Forms.DataGridView dataGridMovies;
        private System.Windows.Forms.DataGridView dataGridShows;
        private System.Windows.Forms.DataGridView dataGridSeats;
        private System.Windows.Forms.Panel panelSeats;   // panel để vẽ sơ đồ ghế

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.txtMovieId = new System.Windows.Forms.TextBox();
            this.txtShowId = new System.Windows.Forms.TextBox();
            this.txtSeats = new System.Windows.Forms.TextBox();

            this.btnListMovies = new System.Windows.Forms.Button();
            this.btnListShows = new System.Windows.Forms.Button();
            this.btnViewSeats = new System.Windows.Forms.Button();
            this.btnBook = new System.Windows.Forms.Button();
            this.btnRelease = new System.Windows.Forms.Button();

            this.dataGridMovies = new System.Windows.Forms.DataGridView();
            this.dataGridShows = new System.Windows.Forms.DataGridView();
            this.dataGridSeats = new System.Windows.Forms.DataGridView();
            this.panelSeats = new System.Windows.Forms.Panel();

            this.SuspendLayout();

            // txtMovieId
            this.txtMovieId.Location = new System.Drawing.Point(20, 20);
            this.txtMovieId.Name = "txtMovieId";
            this.txtMovieId.PlaceholderText = "Movie ID";
            this.txtMovieId.Size = new System.Drawing.Size(200, 23);

            // txtShowId
            this.txtShowId.Location = new System.Drawing.Point(20, 50);
            this.txtShowId.Name = "txtShowId";
            this.txtShowId.PlaceholderText = "Show ID";
            this.txtShowId.Size = new System.Drawing.Size(200, 23);

            // txtSeats
            this.txtSeats.Location = new System.Drawing.Point(20, 80);
            this.txtSeats.Name = "txtSeats";
            this.txtSeats.PlaceholderText = "Seats (A1,A2,...)";
            this.txtSeats.Size = new System.Drawing.Size(200, 23);

            // btnListMovies
            this.btnListMovies.Location = new System.Drawing.Point(250, 20);
            this.btnListMovies.Size = new System.Drawing.Size(100, 30);
            this.btnListMovies.Text = "List Movies";
            this.btnListMovies.Click += new System.EventHandler(this.btnListMovies_Click);

            // btnListShows
            this.btnListShows.Location = new System.Drawing.Point(250, 60);
            this.btnListShows.Size = new System.Drawing.Size(100, 30);
            this.btnListShows.Text = "List Shows";
            this.btnListShows.Click += new System.EventHandler(this.btnListShows_Click);

            // btnViewSeats
            this.btnViewSeats.Location = new System.Drawing.Point(250, 100);
            this.btnViewSeats.Size = new System.Drawing.Size(100, 30);
            this.btnViewSeats.Text = "View Seats";
            this.btnViewSeats.Click += new System.EventHandler(this.btnViewSeats_Click);

            // btnBook
            this.btnBook.Location = new System.Drawing.Point(250, 140);
            this.btnBook.Size = new System.Drawing.Size(100, 30);
            this.btnBook.Text = "Book Seats";
            this.btnBook.Click += new System.EventHandler(this.btnBook_Click);

            // btnRelease
            this.btnRelease.Location = new System.Drawing.Point(250, 180);
            this.btnRelease.Size = new System.Drawing.Size(100, 30);
            this.btnRelease.Text = "Release Seats";
            this.btnRelease.Click += new System.EventHandler(this.btnRelease_Click);

            // dataGridMovies
            this.dataGridMovies.Location = new System.Drawing.Point(20, 120);
            this.dataGridMovies.Size = new System.Drawing.Size(500, 120);
            this.dataGridMovies.Name = "dataGridMovies";

            // dataGridShows
            this.dataGridShows.Location = new System.Drawing.Point(20, 250);
            this.dataGridShows.Size = new System.Drawing.Size(500, 120);
            this.dataGridShows.Name = "dataGridShows";

            // dataGridSeats
            this.dataGridSeats.Location = new System.Drawing.Point(20, 380);
            this.dataGridSeats.Size = new System.Drawing.Size(500, 150);
            this.dataGridSeats.Name = "dataGridSeats";

            // panelSeats
            this.panelSeats.Location = new System.Drawing.Point(550, 20);
            this.panelSeats.Size = new System.Drawing.Size(500, 500);
            this.panelSeats.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // Form1
            this.ClientSize = new System.Drawing.Size(1100, 600);
            this.Controls.Add(this.txtMovieId);
            this.Controls.Add(this.txtShowId);
            this.Controls.Add(this.txtSeats);

            this.Controls.Add(this.btnListMovies);
            this.Controls.Add(this.btnListShows);
            this.Controls.Add(this.btnViewSeats);
            this.Controls.Add(this.btnBook);
            this.Controls.Add(this.btnRelease);

            this.Controls.Add(this.dataGridMovies);
            this.Controls.Add(this.dataGridShows);
            this.Controls.Add(this.dataGridSeats);
            this.Controls.Add(this.panelSeats);

            this.Text = "Cinema Client";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
