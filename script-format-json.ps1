function FormatJson {
    param (
        [Parameter(Mandatory = $true)]
        [string]$Path
    )

    # Lê todas as linhas do arquivo.
    $linhas = Get-Content $Path
    $linhasCorrigidas = @()

    for ($i = 0; $i -lt $linhas.Count; $i++) {

        $linhaAtual = $linhas[$i]

        # Verifica se a linha termina com }, ou ],
        if ($linhaAtual -match '^\s*[\}\]]\s*,\s*$') {
            $proximaLinha = if ($i + 1 -lt $linhas.Count) { $linhas[$i + 1] } else { "" }

            # Se a próxima linha começa com " ou {, mantém a vírgula
            if ($proximaLinha -match '^\s*(\"|\{)') {
                $linhasCorrigidas += $linhaAtual
            } else {
                # Caso contrário, remove a vírgula
                $linhaCorrigida = $linhaAtual -replace ',', ''
                $linhasCorrigidas += $linhaCorrigida
            }
        }
        elseif ($linhaAtual -match '^\s*[\}\]]\s*$') {

            # Se a linha anterior termina com vírgula, remove
            if ($linhasCorrigidas.Count -gt 0) {

                $linhaAnteriorIndex = $linhasCorrigidas.Count - 1
                $linhaAnterior = $linhasCorrigidas[$linhaAnteriorIndex]

                if ($linhaAnterior -match ',\s*$') {
                    $linhasCorrigidas[$linhaAnteriorIndex] = $linhaAnterior -replace ',\s*$', ''
                }
            }
            $linhasCorrigidas += $linhaAtual
        }
        else {

            $linhasCorrigidas += $linhaAtual
        }
    }

    # Salva as linhas corrigidas no mesmo arquivo
    $linhasCorrigidas | Set-Content $Path
}

# Remove o script após execução.
$scriptPath = $MyInvocation.MyCommand.Path
Remove-Item -Path $scriptPath -Force