const { github } = require('@actions/github');
const { request } = require("@octokit/request");

const [owner, repo] = process.env.GITHUB_REPOSITORY.split("/");

const environment = 'stage'

main()

async function main() {
  // See https://docs.github.com/en/free-pro-team@latest/rest/reference/repos#create-a-deployment
  const { data } = await request("POST /repos/:owner/:repo/deployments", {
    owner,
    repo,
    ref: github.context.ref,
    environment,
  });
  console.log("Deployment created: %s", data);
}
