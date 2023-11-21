<?php

// Require the connection file
require('connection.php');

// Check if the user is logged in
if (!isset($_SESSION['user_id'])) {
?>
    <!-- HTML code for not logged in -->
    <link rel="stylesheet" href="styles/footer-style.css">
    <div class="footer">
        <div class="row">
            <div class="col-lg-5">
                <p>Ticket To Ride</p>
                <a href="leaderboard.php">LeaderBoard</a>
                <a href="forum.php">Forum</a>
            </div>
            <div class="col-lg-6">
                <p>Policies</p>
                <a href="https://www.boardgamecapital.com/ticket-to-ride-rules.htm">Rules</a>
                <a href="login.php">Login</a>
                <a href="contact.php">Contact</a>
            </div>
            <div class="col-lg-1">
                <p>Join</p>
                <a href="login.php">Login</a>
                <a href="contact.php">Contact</a>
            </div>
        </div>
    </div>
<?php
} else {
    // Code to execute when the user is logged in
    // You can add more HTML code or redirect to a different page as needed
?>
    <!-- HTML code for logged in -->
    <link rel="stylesheet" href="styles/footer-style.css">
    <div class="footer">
        <div class="row">
            <!--We use "col-lg-6" because we are using a display over 1200px. The 6 stands for how many colums bootstrap need to generate-->
            <div class="col-lg-5">
                <p>Ticket To Ride</p>
                <a href="leaderboard.php">LeaderBoard</a>
                <a href="forum.php">Forum</a>
                <a href="contact.php">Contact</a>
            </div>
            <div class="col-lg-6">
                <p>Policies</p>
                <a href="https://www.boardgamecapital.com/ticket-to-ride-rules.html">Rules</a>
                <a href="profile_page.php">Profile</a>
                <a href="contact.php">Contact</a>
            </div>
        </div>
    </div>
<?php
}
?>