Feature: Book a movie
	Given this list of movies
  I want to book a movie
  So that it is visible as an option for customers to purchase tickets

Background: CurrentBookings list
	Given list of CurrentBookingsBooked
		| date    | title                                        |
		| 12/2021 | Star Wars: Episode VI - Return of the Jedi   |
		#| 1/2022  | Star Wars: Episode VII - Return of the Jedi  |
		#| 2/2022  | Star Wars: Episode VIII - Return of the Jedi |

@Ready @ComponentTest @AcceptanceTest
Scenario: Create a movie booking within the next 3 months and booking does not already exist - Admin
	Given I want to book <title> on date <date>
	When I am booking as an admin user
	Then the booking will be created

	Examples:
		| date    | title                                       |
		| 202112 | Star Wars: Episode VI - Return of the Jedi   |

#@Ready @ComponentTest @AcceptanceTest
#Scenario: Create a movie booking within the next 3 months and booking does already exist - Admin
#	Given I want to book <title> on date <date>
#	When I am booking as an admin user
#	Then the booking will not be created
#	Examples:
#		| date    | title                                       |
#		| 202112 | Star Wars: Episode IV - A New Hope		    |
#
##And the error message is "movie already exists"
#@Ready @ComponentTest @AcceptanceTest
#Scenario: Create a movie booking within the next 3 months and unauthorized - Non-Admin
#	Given I want to book Movie3 on date 202112
#	When I am booking as an nonadmin user
#	Then the booking will not be created
#And the error message is "unauthorized - you are not an admin"