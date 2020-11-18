const github = require('@actions/github');
const core = require('@actions/core');

const octokit = new Octokit();
const [owner, repo] = process.env.GITHUB_REPOSITORY.split("/");

const environment = 'stage'

main()

async function main() {
  // See https://docs.github.com/en/free-pro-team@latest/rest/reference/repos#create-a-deployment
  const { data } = await octokit.request("POST /repos/:owner/:repo/deployments", {
    owner,
    repo,
    ref: github.context.ref,
    environment,
  });
  console.log("Deployment created: %s", data);
}
