<?php
// Starting session to manage user sessions.
session_start();
// Requiring the connection file for database connectivity.
require('connection.php');
?>
<!DOCTYPE html>
<html lang="en">

<head>

  <!-- Setting character set and viewport for better rendering on various devices -->
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">

  <!-- Linking external stylesheet for styling -->
  <link rel="stylesheet" href="styles/frontpage-style.css">
  <!-- Setting the title of the webpage -->
  <title>Home</title>
</head>

<body>

  <?php
  // Including the navbar.php script for consistent navigation across pages.
  require('navbar.php');
  ?>
  <!-- Banner section of the webpage -->
  <div class="banner">
  </div>

  <!-- Information section with a brief overview of the application -->
  <div class="info">
    <h1 class="MT">Ticket To Ride</h1>
    <p class="MP">From the craggy hillsides of Edinburgh to the sunlit docks of Constantinople, <br> from the dusty
      alleys of
      Pamplona to
      a windswept station in Berlin, <br> Ticket to Ride Europe takes you on an exciting train adventure through <br>
      the
      great
      cities of turn-of-the-century Europe.</p>
  </div>

  <!-- About section providing detailed information about Ticket To Ride -->
  <div class="AG">
    <h1 class="AT">About Ticket To Ride</h1>
    <p class="AP">I need A lot of text XDDD <br>
      Lorem ipsum dolor sit amet, consectetur adipisicing elit. Omnis vero vitae at facere id, odit reiciendis non.
      Cumque
      rem consequatur explicabo eveniet quis, soluta sit iste id nostrum impedit esse?<br>
      Lorem, ipsum dolor sit amet consectetur adipisicing elit. Placeat, deleniti iste repellendus quibusdam nam tenetur
      molestias, aliquid totam doloremque magnam aliquam adipisci corrupti eaque, dolorem dolor? Sit officiis animi
      quam?
      Lorem ipsum dolor sit amet consectetur adipisicing elit. Placeat atque perspiciatis aliquid vel ea quo adipisci
      aliquam quaerat voluptatum ratione excepturi quas iusto nihil fugiat, doloremque molestiae facilis, explicabo
      eveniet. <br><br>
      Lorem ipsum dolor sit amet consectetur adipisicing elit. Vel aliquam nihil nobis id excepturi corrupti iusto.
      Distinctio veritatis architecto labore pariatur vel? Neque quibusdam nesciunt rerum voluptate itaque et corporis.
      Lorem ipsum dolor sit amet consectetur adipisicing elit. Asperiores impedit non exercitationem et, natus id
      reprehenderit enim amet laboriosam qui commodi itaque, porro eos repellendus doloremque, recusandae consequuntur?
      Veritatis, ipsam. <br><br>
      Lorem, ipsum dolor sit amet consectetur adipisicing elit. Natus vero, expedita quam nam blanditiis, alias, magni
      nulla vitae doloribus adipisci dicta maxime. Sed aperiam molestias eos dicta eligendi reprehenderit distinctio.
      Lorem ipsum dolor sit, amet consectetur adipisicing elit. <br><br> Maiores dolore distinctio, quis nobis magnam
      officia
      blanditiis! Corrupti quas illum rerum vel officiis quae nostrum, amet culpa, ullam recusandae quisquam
      ipsum?Lorem,
      ipsum dolor sit amet consectetur adipisicing elit. <br><br> Ad veniam ipsam excepturi pariatur natus explicabo
      optio
      voluptate facere necessitatibus sapiente ea magnam ullam id quo omnis culpa aperiam, suscipit earum!loginlo
      Lorem ipsum dolor sit amet consectetur adipisicing elit. Quia veritatis sint aliquid doloremque, ullam dignissimos
      distinctio molestias cum vitae neque unde corrupti iusto placeat atque. Ratione libero autem accusamus>
    </p>
  </div>

  <!-- Button linking to an external resource for learning more about Ticket To Ride -->
  <div class="button">
    <a href="https://www.boardgamecapital.com/ticket-to-ride-rules.htm" class="AB">Learn more</a>
  </div>

  <!-- Game Rules section with an embedded YouTube video and additional information -->
  <div class="GR">
    <h1 class="RB">Rules</h1>
    <iframe class="RV" width="1220" height="980" src="https://www.youtube.com/embed/yPWqKkMKz3E" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>
    <p class="RT"> We linked to a short youtube video explaining how the rules work. <br>This video can be usefull
      for every
      one such as
      new players, experienced players and returning players</p>

    <!-- Link to download Ticket To Ride Official Rules -->
    <a href="https://www.boardgamecapital.com/ticket-to-ride-rules.htm" class="DRB">Download Ticket To Ride Official
      Rules</a>
  </div>

  <?php
  // Including the footer.php script for consistent footer across pages.
  require('footer.php');
  ?>


</body>

</html>