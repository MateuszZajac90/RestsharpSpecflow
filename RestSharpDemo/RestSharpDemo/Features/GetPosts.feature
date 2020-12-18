Feature: GetPosts

Background:
	Given I get JWT authentication of User with following details
		| Email            | Password |
		| nilson@email.com | nilson   |

Scenario: Verify author of the posts 1
	Given I perform GET operation for "posts/{postid}"
	And I perform operation for post "1"
	Then I should see the "author" name as "Mateusz"

Scenario: Verify author of the posts 2
	Given I perform GET operation for "posts/{postid}"
	And I perform operation for post "2"
	Then I should see the "author" name as "Adam"

Scenario: Verify author of the posts 4
	Given I perform GET operation for "posts/{postid}"
	And I perform operation for post "4"
	Then I should see the "author" name as "Bogdan"