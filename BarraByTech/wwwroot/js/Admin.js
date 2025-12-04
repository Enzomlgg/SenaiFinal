// VARIÁVEIS GLOBAIS
const API_URL = 'https://localhost:7289/api/Produtos';
const produtoForm = document.getElementById('produto-form');
const tabelaProdutosBody = document.querySelector('#tabela-produtos tbody');
const produtoIdInput = document.getElementById('produto-id');
const formTitle = document.getElementById('form-title');
const submitBtn = document.getElementById('submit-btn');
const cancelEditBtn = document.getElementById('cancel-edit-btn');
const selectMarca = document.getElementById('select-marca');
const selectTipoMarca = document.getElementById('select-tipomarca');
const selectCategoria = document.getElementById('select-categoria');
const tiposDetail = document.getElementById('tipos-marca-detalhe');

// Variáveis de Dados
let todasMarcas = [];
let todasCategorias = [];
let todasTiposMarca = [];

// Funções Auxiliares
const parseGuid = (value) => {
    // Retorna o GUID se for válido, ou Guid.Empty para o C#
    return value && value !== '00000000-0000-0000-0000-000000000000' ? value : '00000000-0000-0000-0000-000000000000';
};
function parseCurrency(value) {
    if (typeof value === 'string') {
        value = value.replace('R$', '').replace(/\./g, '').replace(',', '.').trim();
    }
    return parseFloat(value) || 0;
}

// -- FUNÇÕES DE GERAL E INICIALIZAÇÃO --

window.openTab = function (evt, tabName) { //
    var i, tabcontent, tablinks;
    tabcontent = document.getElementsByClassName("tab-content");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].classList.remove("active"); //
    }
    tablinks = document.getElementsByClassName("sidebar-btn"); //
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }
    document.getElementById(tabName).classList.add("active"); //
    evt.currentTarget.className += " active";
    localStorage.setItem('activeAdminTab', tabName); //
}

function carregarDadosIniciais() { //
    Promise.all([
        fetch('/Admin/GetMarcas').then(r => r.json()).catch(() => []),
        fetch('/Admin/GetTodasCategorias').then(r => r.json()).catch(() => []),
        fetch('/Admin/GetTodosTiposMarca').then(r => r.json()).catch(() => [])
    ])
        .then(([marcas, categorias, tiposMarca]) => {
            todasMarcas = marcas; //
            todasCategorias = categorias;
            todasTiposMarca = tiposMarca; //
            fetchProdutos();
        })
        .catch(e => console.error("Erro ao carregar dados iniciais:", e)); //
}

document.addEventListener("DOMContentLoaded", function () { //
    var activeTab = localStorage.getItem('activeAdminTab') || 'tab-produtos';
    var btn = document.querySelector(`.sidebar-btn[onclick*="'${activeTab}'"]`);
    var content = document.getElementById(activeTab);

    if (btn && content) {
        document.querySelectorAll('.tab-content').forEach(el => el.classList.remove('active'));
        document.querySelectorAll('.sidebar-btn').forEach(el => el.classList.remove('active'));

        content.classList.add('active');
        btn.classList.add('active');
    }

    carregarDadosIniciais();
});

// ----------------------------------------------------------------------
// -- FUNÇÕES DE PRODUTO (MODO FORMULÁRIO) --
// ----------------------------------------------------------------------

function setFormModeEdicao(produto) { // 
    formTitle.textContent = `Editar Produto: ${produto.nome}`; // 
    produtoForm.action = '/Admin/EditarProduto'; // [cite: 13]
    produtoIdInput.value = produto.produtoId; // [cite: 14]
    submitBtn.textContent = 'Salvar Produto';
    submitBtn.style.backgroundColor = '#2ecc71';
    cancelEditBtn.style.display = 'block';

    document.getElementById('nome-produto').value = produto.nome || ''; // [cite: 15]
    document.getElementById('descricao-produto').value = produto.descricao || '';

    document.getElementById('boleto-valor').value = produto.boleto || 0; // [cite: 16]
    document.getElementById('cartao-valor').value = produto.cartao || 0;
    document.getElementById('vista-valor').value = produto.vista || 0; // [cite: 17]
    document.getElementById('promocao-valor').value = produto.promocao || 0;

    document.getElementById('estoque-min').value = produto.estoqueMin || 0; // [cite: 18]
    document.getElementById('estoque-atual').value = produto.estoqueAtual || 0; // [cite: 19]

    selectCategoria.value = produto.categoriaId || '';
    selectMarca.value = produto.marcaId || ''; // [cite: 20]

    if (produto.marcaId) {
        carregarTiposParaSelect(produto.marcaId, produto.tipoMarcaId); // [cite: 20]
    }

    document.getElementById('tab-produtos').scrollIntoView({ behavior: 'smooth' });
}

window.setFormModeCriacao = function () { // [cite: 22]
    formTitle.textContent = 'Cadastrar Novo Produto'; // [cite: 22]
    produtoForm.action = '/Admin/CriarProduto'; // [cite: 23]
    produtoIdInput.value = '';
    submitBtn.textContent = 'Cadastrar Produto';
    submitBtn.style.backgroundColor = '#0078D7';
    cancelEditBtn.style.display = 'none';

    produtoForm.reset(); // [cite: 23]

    // Lógica de Imagem removida do original [cite: 24, 25]

    selectTipoMarca.innerHTML = '<option value="">-- Selecione a Marca primeiro --</option>'; // [cite: 25]
    selectTipoMarca.disabled = true; // [cite: 26]

    produtoForm.querySelectorAll('.campo-erro').forEach(el => el.classList.remove('campo-erro'));
};

cancelEditBtn.addEventListener('click', function (e) { // [cite: 26]
    e.preventDefault();
    setFormModeCriacao();
});

// ----------------------------------------------------------------------
// -- FUNÇÕES DE PRODUTO (CRUD LÓGICA) --
// ----------------------------------------------------------------------

produtoForm.addEventListener('submit', async function (e) { // 
    e.preventDefault();

    const isEditing = produtoIdInput.value !== '';
    const url = isEditing ? `${API_URL}/${produtoIdInput.value}` : API_URL;
    const method = isEditing ? 'PUT' : 'POST'; // 

    const produtoBody = {
        ProdutoId: parseGuid(produtoIdInput.value), // [cite: 43]
        Nome: document.getElementById('nome-produto').value || "", // [cite: 44]
        Descricao: document.getElementById('descricao-produto').value || "", // [cite: 45]

        Boleto: parseCurrency(document.getElementById('boleto-valor').value),
        Cartao: parseCurrency(document.getElementById('cartao-valor').value),
        Vista: parseCurrency(document.getElementById('vista-valor').value),
        Promocao: parseCurrency(document.getElementById('promocao-valor').value),

        EstoqueMin: parseInt(document.getElementById('estoque-min').value) || 0, // [cite: 46]
        EstoqueAtual: parseInt(document.getElementById('estoque-atual').value) || 0, // [cite: 47]

        CategoriaId: parseGuid(selectCategoria.value),
        MarcaId: parseGuid(selectMarca.value),
        TipoMarcaId: parseGuid(selectTipoMarca.value),

        CaminhosImagens: [], // OBRIGATÓRIO (Array Vazio) [cite: 47]
    };

    try { // [cite: 49]
        const response = await fetch(url, {
            method: method,
            headers: {
                'Content-Type': 'application/json' // ESSENCIAL
            },
            body: JSON.stringify(produtoBody) // [cite: 50]
        });

        if (response.ok) { // [cite: 51]
            alert(`Produto ${isEditing ? 'atualizado' : 'cadastrado'} com sucesso!`); // [cite: 51]
            setFormModeCriacao(); // [cite: 52]
            fetchProdutos(); // Assumindo que você tem uma função para recarregar a tabela
        } else { // [cite: 53]
            const errorText = await response.text();
            alert(`Erro ao salvar produto (${response.status}): ${errorText}`); // [cite: 54]
        }
    } catch (error) {
        console.error('Erro de rede ou na requisição:', error);
        alert('Erro ao se comunicar com a API. Verifique o console.'); // [cite: 55]
    }
});

// 3.2 CARREGAMENTO PARA EDIÇÃO E REMOÇÃO (Requisição Direta)

window.carregarProdutoParaEdicao = function (produtoId) { // [cite: 72]
    setFormModeCriacao();

    fetch(`/Admin/GetProdutoCompleto?produtoId=${produtoId}`)
        .then(r => {
            if (!r.ok) throw new Error('Falha ao buscar produto completo: ' + r.statusText);
            return r.json();
        })
        .then(produto => {
            setFormModeEdicao(produto);
        })
        .catch(e => { // [cite: 73]
            console.error("Erro ao carregar produto para edição:", e);
            alert('Erro ao carregar produto. Verifique o console.');
        });
};

window.removerProduto = function (produtoId, nome) {
    if (!confirm(`Tem certeza que deseja remover o produto "${nome || 'sem nome'}"?`)) {
        return;
    }

    // Adaptado para usar DELETE no endpoint da API
    fetch(`${API_URL}/${produtoId}`, {
        method: 'DELETE',
    })
        .then(r => {
            // Verifica 200 OK ou 204 No Content (retorno comum para DELETE)
            if (r.ok || r.status === 204) {
                alert('Produto removido com sucesso!');
                fetchProdutos(); // Recarrega a tabela via AJAX, sem recarregar a página
            } else {
                alert('Falha ao remover o produto.');
            }
        })
        .catch(e => {
            console.error("Erro ao remover produto:", e);
            alert('Erro de rede ao remover.');
        });
};

// ----------------------------------------------------------------------
// -- FUNÇÕES DE TABELA DE PRODUTOS (VISUALIZAÇÃO) --
// ----------------------------------------------------------------------

function fetchProdutos() { // [cite: 65]
    fetch('/Admin/GetProdutosData')
        .then(r => r.json())
        .then(produtos => {
            renderizarTabelaProdutos(produtos);
        })
        .catch(e => {
            console.error("Erro ao buscar produtos:", e);
            tabelaProdutosBody.innerHTML = `<tr><td colspan="6" style="text-align:center;">Erro ao carregar lista de produtos.</td></tr>`; // [cite: 66]
        });
}

function renderizarTabelaProdutos(produtos) { // [cite: 66]
    tabelaProdutosBody.innerHTML = '';

    const getNomeById = (list, id, idField, nameField) => { // [cite: 67]
        const item = list.find(i => i[idField] && i[idField].toLowerCase() === (id || '').toLowerCase());
        return item ? item[nameField] : 'N/A'; // [cite: 68]
    };

    if (produtos && produtos.length > 0) {
        produtos.forEach(p => {
            const row = tabelaProdutosBody.insertRow();

            const categoriaNome = getNomeById(todasCategorias, p.categoriaId, 'categoriaId', 'categoriaNome');
            const marcaNome = getNomeById(todasMarcas, p.marcaId, 'marcaId', 'nome');
            const tipoNome = getNomeById(todasTiposMarca, p.tipoMarcaId, 'tipoMarcaId', 'nome');

            row.insertCell().textContent = p.produtoId ? p.produtoId.substring(0, 8) + '...' : 'N/A'; // [cite: 69]
            row.insertCell().textContent = p.nome || '';
            row.insertCell().textContent = categoriaNome;
            row.insertCell().textContent = marcaNome;
            row.insertCell().textContent = tipoNome;

            const acoesCell = row.insertCell();
            acoesCell.innerHTML = `
                <button onclick="carregarProdutoParaEdicao('${p.produtoId}')" style="background-color: #0078D7;">Editar</button>
                <button onclick="removerProduto('${p.produtoId}', '${p.nome}')" class="btn-danger">Remover</button>
            `; // [cite: 70]
        }); // [cite: 71]
    } else {
        tabelaProdutosBody.innerHTML = '<tr><td colspan="6" style="text-align:center;">Nenhum produto cadastrado.</td></tr>'; // [cite: 72]
    }
}

// ----------------------------------------------------------------------
// -- FUNÇÕES DE MARCA/TIPO (SELECTS E MESTRE-DETALHE) --
// ----------------------------------------------------------------------

selectMarca.addEventListener('change', function () { // [cite: 56]
    const marcaId = this.value;
    carregarTiposParaSelect(marcaId, null);
});

function carregarTiposParaSelect(marcaId, tipoMarcaIdPreSelecionado) { // [cite: 57]
    selectTipoMarca.innerHTML = '<option value="">Carregando...</option>'; // [cite: 57]
    selectTipoMarca.disabled = true;

    if (marcaId && marcaId !== "") { // [cite: 58]
        const url = '/Admin/GetTiposMarca?marcaId=' + marcaId; // [cite: 58]

        fetch(url) // [cite: 59]
            .then(response => response.json())
            .then(tipos => {
                selectTipoMarca.innerHTML = '<option value="">-- Selecione um Tipo --</option>';

                if (tipos && tipos.length > 0) {
                    tipos.forEach(tipo => { // [cite: 60]
                        const option = document.createElement('option');
                        option.value = tipo.tipoMarcaId;
                        option.textContent = tipo.nome;
                        if (tipoMarcaIdPreSelecionado && tipo.tipoMarcaId.toLowerCase() === tipoMarcaIdPreSelecionado.toLowerCase()) { // [cite: 61]
                            option.selected = true;
                        }
                        selectTipoMarca.appendChild(option);
                    }); // [cite: 62]
                    selectTipoMarca.disabled = false;
                } else {
                    selectTipoMarca.innerHTML = '<option value="">Nenhum Tipo encontrado</option>'; // [cite: 62]
                }
            })
            .catch(error => { // [cite: 63]
                console.error('Erro de API/Rede ao carregar Tipos:', error);
                selectTipoMarca.innerHTML = '<option value="">Erro ao carregar Tipos</option>'; // [cite: 64]
            });
    } else {
        selectTipoMarca.innerHTML = '<option value="">-- Selecione a Marca primeiro --</option>'; // [cite: 64]
    } // [cite: 65]
}

window.carregarTiposMarca = carregarTiposMarca; // [cite: 78]
function carregarTiposMarca(marcaId, marcaNome) { // [cite: 79]
    tiposDetail.classList.add('hidden');
    tiposDetail.innerHTML = `<h3>Carregando Tipos de ${marcaNome}...</h3><p>Aguarde...</p>`;
    tiposDetail.classList.remove('hidden'); // [cite: 79]

    const url = '/Admin/GetTiposMarca?marcaId=' + marcaId; // [cite: 80]

    fetch(url) // [cite: 80]
        .then(response => response.json())
        .then(tipos => {
            todasTiposMarca = todasTiposMarca.filter(t => t.marcaId.toLowerCase() !== marcaId.toLowerCase()).concat(tipos);

            const content = gerarConteudoTipos(marcaId, marcaNome, tipos);
            tiposDetail.innerHTML = content;
        })
        .catch(error => { // [cite: 81]
            console.error('Erro de API/Rede:', error);
            tiposDetail.innerHTML = '<h3>Erro ao carregar Tipos de Marca.</h3><p>Verifique a conexão ou logs do servidor.</p>';
        }); // [cite: 82]
}

function gerarConteudoTipos(marcaId, marcaNome, tipos) { // [cite: 82]
    let html = `
        <h3 style="color:#1abc9c;">Gerenciar Tipos: ${marcaNome}</h3>
        
        <h4>Criar Novo Tipo</h4>
        <form method="post" action="/Admin/CriarTipoMarca" class="form-tipo">
            <input type="hidden" name="MarcaId" value="${marcaId}" />
            <input type="text" name="Nome" placeholder="Nome do Tipo (Ex: Core i7)" required />
            <button type="submit" style="width:100%;">Criar Tipo</button>
        </form>

        <hr />

        <h4>Tipos existentes (${marcaNome})</h4>
    `; // [cite: 83]

    if (tipos && tipos.length > 0) { // [cite: 84]
        html += `
            <table>
                <thead>
                    <tr>
                        <th>Tipo</th>
                        <th style="width: 200px;">Ações</th>
                    </tr>
                </thead>
                <tbody>
        `; // [cite: 85]

        tipos.forEach(t => { // [cite: 86]
            html += `
                <tr>
                    <td>
                        <input type="text" form="form-edit-tipo-${t.tipoMarcaId}" name="Nome" value="${t.nome}" required />
                    </td>
                    <td>
                        <form id="form-edit-tipo-${t.tipoMarcaId}" method="post" action="/Admin/EditarTipoMarca" style="display:inline;">
                            <input type="hidden" name="TipoMarcaId" value="${t.tipoMarcaId}" />
                            <input type="hidden" name="MarcaId" value="${marcaId}" />
                            <button type="submit">Salvar</button>
                        </form>

                        <form method="post" action="/Admin/RemoverTipoMarca" style="display:inline;">
                            <input type="hidden" name="tipoMarcaId" value="${t.tipoMarcaId}" />
                            <button type="submit" class="btn-danger" onclick="return confirm('Tem certeza que deseja remover o tipo ${t.nome}?');">Remover</button>
                        </form>
                    </td>
                </tr>
            `; // [cite: 90]
        }); // [cite: 91]

        html += `
                </tbody>
            </table>
        `; // [cite: 92]
    } else {
        html += `<p>Nenhum Tipo de Marca cadastrado para **${marcaNome}**.</p>`; // [cite: 92]
    } // [cite: 93]

    return html;
}