Feature: PostProfile

@smoke
Scenario: Verify Post operation for Profile
	Given I Perform POST operation for "/posts/{profileNo}/profile" with body 
	| name | profile |
	| Mams | 3       |
	Then I should see the "name" name as "Mams"