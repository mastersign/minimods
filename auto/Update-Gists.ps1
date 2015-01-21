$myDir = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
$baseDir = Resolve-Path "$myDir\.."
$tmpDir = "$baseDir\tmp_repos"

$baseNamespace = "Mastersign.Minimods"
$codeFileExt = "cs"
$remoteStr1 = "git@gist.github.com:/"
$remoteStr2 = ".git"

Set-Location $myDir

if (!(Get-Command "git" -ErrorAction SilentlyContinue)) {
    throw "git is not on path"
}

function cleanup-dir($trgDir) {
    if (Test-Path $trgDir) {
        rm $trgDir -Recurse -Force
    }
    mkdir $trgDir | Out-Null
}

function checkout-gist($trgDir, $gistId) {
    Push-Location $trgDir

    $remote = $remoteStr1 + $gistId + $remoteStr2
    git clone -q $remote .

    Pop-Location
}

function update-gist-content($trgDir, $id, $codeFile, $testFile) {
    Push-Location $trgDir

    Remove-Item "*.$codeFileExt"
    Copy-Item $codeFile .
    if (Test-Path $testFile) {
        Copy-Item $testFile .
    }

    Pop-Location
}

function has-changed($trgDir) {
    Push-Location $trgDir

    $output = git status --porcelain
    
    Pop-Location
    if ($output.Count -gt 0) {
        return $true
    } else {
        return $false
    }
}

function commit-and-push-gist($trgDir) {
    Push-Location $trgDir

    git add -A
    git commit -q -m "Auto Update"
    git push -q

    Pop-Location
}

$indexFile = Resolve-Path "$baseDir\index.xml"
if (!$indexFile) { return }

[xml]$index = Get-Content $indexFile

cleanup-dir $tmpDir

$cnt = 0
foreach($minimod in $index.Minimods.Minimod) {
    $cnt = $cnt + 1
    $id = $minimod.id
    $gistId = $minimod.Gist

    echo "Updating Gist `"$id`" [$gistId] ..."
    
    $codeFile = "$baseDir\$baseNamespace.$id.$codeFileExt"
    $testFile = "$baseDir\$baseNamespace.$id.Test.$codeFileExt"
    
    $repoDir = "$tmpDir\$gistId"
    mkdir $repoDir | Out-Null

    checkout-gist $repoDir $gistId
    update-gist-content $repoDir $id $codeFile $testFile

    if (has-changed $repoDir) {
        echo "... changes found ..."
        commit-and-push-gist $repoDir
        echo "... changes published."
    } else {
        echo "... no changes found."
    }
}

Remove-Item $tmpDir -Recurse -Force

echo "Finished."