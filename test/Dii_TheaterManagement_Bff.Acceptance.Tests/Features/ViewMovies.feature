Feature: ViewMovies
	View the list of movies

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

@mytag
Scenario: The title can be seen
	When I view the list of movies
	Then the movie list should show
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