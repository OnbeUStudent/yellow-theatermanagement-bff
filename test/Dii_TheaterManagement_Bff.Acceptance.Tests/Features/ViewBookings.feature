@ignore
Feature: View Bookings
  As a user
  I want to view the CurrentBookings
  So that I can tell my consumers what tickets they can purchase

Background:
    Given the following movies
    | Title                                          |
    | Solo: A Star Wars Story                        |
    | Rogue One: A Star Wars Story                   |
    | Star Wars: Episode I - The Phantom Menace      |
    | Star Wars: Episode II - Attack of the Clones   |
    | Star Wars: Episode III - Revenge of the Sith   |
    | Star Wars: Episode IV - A New Hope             |
    | Star Wars: Episode IX - The Rise of Skywalker  |
    | Star Wars: Episode V - The Empire Strikes Back |
    | The Star Wars Holiday Special                  |
    | Star Wars: Episode VI - Return of the Jedi     |
    | Star Wars: Episode VII - The Force Awakens     |
    | Star Wars: Episode VIII - The Last Jedi        |
    | The Star Wars Holiday Special                  |

    Given list of CurrentBookingsView                          
    | date    | title                                          |
    | 202110  | The Star Wars Holiday Special                  |
    | 202112  | Star Wars: Episode VI - Return of the Jedi     |
    | 202111  | Star Wars: Episode V - The Empire Strikes Back |
    | 202109  | Star Wars: Episode IV - A New Hope             |
    

  @Ready @AcceptanceTest @ComponentTest
  Scenario: View Bookings - admin
    When I view bookings as an user
    Then I am able to see all CurrentBookings
