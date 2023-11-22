// Use jQuery to handle the click event after the document is ready
$(document).ready(function () {

    // Attach a click event handler to the element with id 'executeLink'
    $('#executeLink').on('click', function (e) {
  
      // Prevent the default behavior of the link
      e.preventDefault();
  
      // Make an AJAX request to check login status
      $.ajax({
        type: 'POST', // You can also use 'GET' depending on your needs
        url: 'check_login.php', // Replace with the actual file path
  
        // Callback function to handle the AJAX request's success
        success: function (response) {
          // Log the server response to the console
          console.log(response);
  
          // Handle the response from the server
          if (response === 'loggedin') {
            // Redirect to the home page if the user is logged in
            window.location.href = 'home.php';
          } else {
            // Redirect to the login page if the user is not logged in
            window.location.href = 'index.php';
          }
        }
      });
    });
  });
  