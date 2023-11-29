<?php
//starting session
session_start();
// require the connection file
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
    // Include the navbar.php script
    require('navbar.php');
    ?>
  </header>

  <!--- We use a form with an action and with an http call its possible to use a api from a company called formspree.io-->
  <form action="https://formspree.io/f/xdojlwro" method="POST">
    <div class="contact">
      <section>
        <div class="topic">
          <p>Topic</p>

          <!---We make a drop down menu with 3 different choices-->
          <select name="CT" id="CT">
            <option value="Account">Account</option>
            <option value="Gameplay">Gameplay</option>
            <option value="Other">Other</option>
          </select>
        </div>

        <!--The 3 following classes is information the user have to type for us to help with the problem-->
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
          <textarea id="MB" name="Message" placeholder="Write your problem here" required></textarea>
        </div>
        <div class="btn">
          <button id="btn" type="submit">Send</button>
        </div>
        <div class="back-link">
      </section>
    </div>
  </form>
  <footer>
    <?php
    // Include the footer.php script
    require('footer.php');
    ?>
  </footer>
</body>

</html>