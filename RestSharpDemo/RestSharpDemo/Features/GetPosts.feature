Feature: GetPosts

Scenario: Verify author of the posts 1
	Given I perform GET operation for "posts/{postid}"
	And I perform operation for post "1"
	Then I should see the "author" name as "Mateusz" 

Scenario: Verify author of the posts 2
	Given I perform GET operation for "posts/{postid}"
	And I perform operation for post "2"
	Then I should see the "author" name as "Zajac" 


Scenario: Verify author of the posts 4
	Given I perform GET operation for "posts/{postid}"
	And I perform operation for post "4"
	Then I should see the "author" name as "Johnny" 