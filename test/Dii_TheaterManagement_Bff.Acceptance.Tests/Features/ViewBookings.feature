Feature: View Bookings
- FIX THIS
  As a user
  I want to view the CurrentBookings
  So that I know what's playing

   Background: CurrentBookings list
    Given list of CurrentBookingsView                          
    | date    | tittle                                                |
    | 202110  | The Star Wars Holiday Special [Fake]                  |
    | 202112  | Star Wars: Episode VI - Return of the Jedi [Fake]     |
    | 202111  | Star Wars: Episode V - The Empire Strikes Back [Fake] |
    | 202109  | Star Wars: Episode IV - A New Hope [Fake]             |

    

  @Ready @AcceptanceTest @ComponentTest
  Scenario: View Bookings - admin
    When I view bookings as an admin user on the admin page
    Then I am able to see all CurrentBookings

# @Ready @AcceptanceTest @ComponentTest
#  Scenario: View Bookings - non-admin
#    When I view bookings as an nonadmin user on the customer page
#    Then I am able to see all CurrentBookings
#
#  @Ready @ComponentTest @AcceptanceTest
#  Scenario: View Bookings after the next 3 months
#    When I view bookings as an nonadmin user on the customer page
#    Then I cannot see ViewBooking link