# Blazor WebAssembly With Playwright and authentication Azure B2C

The goal is to be able to test a Blazor Web Assembly project hostet on asp.net core server with azure active directory b2c.

I'd like to be able to test it in a pipeline, so actually using b2c in the test is not an option.

The issue so far is to bypass / mock or disable the authentication while running the tests


The main setup is a copy from: https://medium.com/younited-tech-blog/end-to-end-test-a-blazor-app-with-playwright-part-2-3980b573e92a
