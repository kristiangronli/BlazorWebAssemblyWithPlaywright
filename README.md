# Blazor WebAssembly With Playwright and authentication Azure B2C

The goal is to be able to test a Blazor Web Assembly project hostet on asp.net core server with azure active directory b2c.

I'd like to be able to test it in a pipeline, so actually using b2c in the test is not an option.

There are 3 different test setups:
1. Playwrigh Auth test - Starts the server. Replaces azure b2c with an inmemory version of identityserver, but this does not reflect to the blazor application. *This is the main issue* - The test fails when we visit an authenticated page
2. Server Api Test - This starts the server with a fake user, and creates a client with the same fake user. *works*
3. Playwright No Auth test - Starts the server with disabled authentication, opens a page that doesn't require authentication using playwright browser, and performes some actions. This "works" but there are no acutall tests at the moment.



I think the main issue is to figure out how to start the blazor application with the substituted identity server.
We should then be able to signin using playwright, and visit the authenticated pages as normal


The main setup is a copy from: https://medium.com/younited-tech-blog/end-to-end-test-a-blazor-app-with-playwright-part-2-3980b573e92a
