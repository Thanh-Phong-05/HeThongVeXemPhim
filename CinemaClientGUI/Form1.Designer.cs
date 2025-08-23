namespace CinemaClientGUI
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.DataGridView dataGridMovies;
        private System.Windows.Forms.DataGridView dataGridShows;
        private System.Windows.Forms.DataGridView dataGridSeats;
        private System.Windows.Forms.Panel panelSeats;

        private System.Windows.Forms.TextBox txtMovieId;
        private System.Windows.Forms.TextBox txtShowId;
        private System.Windows.Forms.TextBox txtSeats;

        private System.Windows.Forms.Button btnListMovies;
        private System.Windows.Forms.Button btnListShows;
        private System.Windows.Forms.Button btnViewSeats;
        private System.Windows.Forms.Button btnBook;
        private System.Windows.Forms.Button btnRelease;

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
            this.dataGridMovies = new System.Windows.Forms.DataGridView();
            this.dataGridShows = new System.Windows.Forms.DataGridView();
            this.dataGridSeats = new System.Windows.Forms.DataGridView();
            this.panelSeats = new System.Windows.Forms.Panel();

            this.txtMovieId = new System.Windows.Forms.TextBox();
            this.txtShowId = new System.Windows.Forms.TextBox();
            this.txtSeats = new System.Windows.Forms.TextBox();

            this.btnListMovies = new System.Windows.Forms.Button();
            this.btnListShows = new System.Windows.Forms.Button();
            this.btnViewSeats = new System.Windows.Forms.Button();
            this.btnBook = new System.Windows.Forms.Button();
            this.btnRelease = new System.Windows.Forms.Button();

            this.SuspendLayout();

            // === Cột trái: Movies ===
            this.dataGridMovies.Location = new System.Drawing.Point(10, 10);
            this.dataGridMovies.Size = new System.Drawing.Size(300, 500);
            this.dataGridMovies.Name = "dataGridMovies";

            this.btnListMovies.Location = new System.Drawing.Point(10, 520);
            this.btnListMovies.Size = new System.Drawing.Size(300, 30);
            this.btnListMovies.Text = "Load Movies";
            this.btnListMovies.Click += new System.EventHandler(this.btnListMovies_Click);

            // === Cột giữa: Thông tin phim + Shows ===
            this.dataGridShows.Location = new System.Drawing.Point(330, 10);
            this.dataGridShows.Size = new System.Drawing.Size(400, 200);
            this.dataGridShows.Name = "dataGridShows";

            this.btnListShows.Location = new System.Drawing.Point(330, 220);
            this.btnListShows.Size = new System.Drawing.Size(190, 30);
            this.btnListShows.Text = "Load Shows";
            this.btnListShows.Click += new System.EventHandler(this.btnListShows_Click);

            this.btnViewSeats.Location = new System.Drawing.Point(540, 220);
            this.btnViewSeats.Size = new System.Drawing.Size(190, 30);
            this.btnViewSeats.Text = "View Seats";
            this.btnViewSeats.Click += new System.EventHandler(this.btnViewSeats_Click);

            this.dataGridSeats.Location = new System.Drawing.Point(330, 270);
            this.dataGridSeats.Size = new System.Drawing.Size(400, 240);
            this.dataGridSeats.Name = "dataGridSeats";

            // Input box để hiển thị ShowId & Seats
            this.txtMovieId.Location = new System.Drawing.Point(330, 520);
            this.txtMovieId.Size = new System.Drawing.Size(190, 23);
            this.txtMovieId.PlaceholderText = "Movie ID";

            this.txtShowId.Location = new System.Drawing.Point(540, 520);
            this.txtShowId.Size = new System.Drawing.Size(190, 23);
            this.txtShowId.PlaceholderText = "Show ID";

            this.txtSeats.Location = new System.Drawing.Point(330, 550);
            this.txtSeats.Size = new System.Drawing.Size(400, 23);
            this.txtSeats.PlaceholderText = "Seats (A1,A2,...)";

            this.btnBook.Location = new System.Drawing.Point(330, 580);
            this.btnBook.Size = new System.Drawing.Size(190, 30);
            this.btnBook.Text = "Book Seats";
            this.btnBook.Click += new System.EventHandler(this.btnBook_Click);

            this.btnRelease.Location = new System.Drawing.Point(540, 580);
            this.btnRelease.Size = new System.Drawing.Size(190, 30);
            this.btnRelease.Text = "Release Seats";
            this.btnRelease.Click += new System.EventHandler(this.btnRelease_Click);

            // === Cột phải: sơ đồ ghế ===
            this.panelSeats.Location = new System.Drawing.Point(750, 10);
            this.panelSeats.Size = new System.Drawing.Size(600, 600);
            this.panelSeats.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            // Form1
            this.ClientSize = new System.Drawing.Size(1380, 640);
            this.Controls.Add(this.dataGridMovies);
            this.Controls.Add(this.btnListMovies);

            this.Controls.Add(this.dataGridShows);
            this.Controls.Add(this.btnListShows);
            this.Controls.Add(this.btnViewSeats);

            this.Controls.Add(this.dataGridSeats);
            this.Controls.Add(this.txtMovieId);
            this.Controls.Add(this.txtShowId);
            this.Controls.Add(this.txtSeats);
            this.Controls.Add(this.btnBook);
            this.Controls.Add(this.btnRelease);

            this.Controls.Add(this.panelSeats);

            this.Text = "Cinema Client - Movie Booking";
            this.Load += new System.EventHandler(this.Form1_Load);

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
