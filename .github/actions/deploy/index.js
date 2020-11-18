const github = require('@actions/github');
const core = require('@actions/core');

const octokit = new Octokit();
const [owner, repo] = process.env.GITHUB_REPOSITORY.split("/");


const environment = 'stage'

// See https://developer.github.com/v3/issues/#create-an-issue
const { data } = await octokit.request("POST /repos/:owner/:repo/deployments", {
  owner,
  repo,
  ref: github.context.ref,
  environment,
});
console.log("Deployment created: %s", data);