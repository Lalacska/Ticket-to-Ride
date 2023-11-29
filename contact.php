<?php
//starting session
session_start();
// Requiring the connection file for database connection
require('connection.php');
?>
<!DOCTYPE html>
<html lang="en">

<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <link rel="stylesheet" href="styles/contact-style.css">
  <title>Contact</title>
</head>

<body>
  <header>
    <?php
    // Including the navbar.php script for the website navigation bar
    require('navbar.php');
    ?>
  </header>

  <!-- Using a form with an action pointing to formspree.io for handling form submissions -->
  <form action="https://formspree.io/f/xdojlwro" method="POST">
    <div class="contact">
      <section>
        <div class="topic">
          <p>Topic</p>

          <!-- Dropdown menu for selecting the topic of the inquiry -->
          <select name="CT" id="CT">
            <option value="Account">Account</option>
            <option value="Gameplay">Gameplay</option>
            <option value="Other">Other</option>
          </select>
        </div>

        <!-- Fields for user information -->
        <div class="FN">
          <p>Full Name</p>
          <input type="text" name="Name" id="FN" placeholder="Enter Name" required />
        </div>
        <div class="EM">
          <p>Email</p>
          <input id="EM" type="email" name="Email" placeholder="Enter Email" required />
        </div>
        <div class="MB">
          <p>What is your problem</p>

          <!-- Textarea for the user to describe their problem -->
          <textarea id="MB" name="Message" placeholder="Write your problem here" required></textarea>
        </div>
        <div class="btn">

          <!-- Submit button to send the form data -->
          <button id="btn" type="submit">Send</button>
        </div>
        <div class="back-link">
      </section>
    </div>
  </form>
  <footer>
    <?php
    // Including the footer.php script for the website footer
    require('footer.php');
    ?>
  </footer>
</body>

</html>