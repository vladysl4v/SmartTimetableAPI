const msalConfig = {
    auth: {
        clientId: "aabcccce-2050-4325-8963-593f5441a728",
        authority: "https://login.microsoftonline.com/cf94ad9d-2983-43f5-9909-722602ea2165",
        redirectUri: "http://localhost:5500/index.html",
        scopes: ["api://6fee3e9d-c90a-44ee-b784-9d0c463a4952/access_as_user"]
    },
    cache: {
        cacheLocation: "localStorage", 
        storeAuthStateInCookie: false,
    }
};

const myMSALObj = new msal.PublicClientApplication(msalConfig);

function authenticateUser() {
    const account = findStoredAccount();
    changeButtonState(account, account ? account.name : " ");
}

function signIn() {
    myMSALObj.loginPopup({ scopes: msalConfig.auth.scopes })
        .then(handleResponse)
        .catch(error => {
            console.error(error);
        });
}

function handleResponse(response) {
    CallApiWithAuthorization("/auth", response, () => {
        changeButtonState(true, response.account.name);
    });
} 

function signOut() {
    myMSALObj.logout({
        account: findStoredAccount(),
        postLogoutRedirectUri: msalConfig.auth.redirectUri,
        mainWindowRedirectUri: msalConfig.auth.redirectUri
    });
    changeButtonState(false);
}

function findStoredAccount() {
    const currentAccounts = myMSALObj.getAllAccounts();
    if (currentAccounts === null) {
        return null;
    } else {
        return currentAccounts[0];
    }
}

function changeButtonState(isAuthorized, accountFullName = ' ') {
    const item = document.createElement("button");
    item.type = 'button';
    item.classList.add('btn');
    if (isAuthorized)
    {
        item.classList.add('btn-primary');
        item.classList.add('fw-bold');
        item.innerText = accountFullName.split(" ")[0];
        item.addEventListener('click', signOut);
    } 
    else {
        item.classList.add('btn-outline-primary');
        item.innerText = 'Увійти';
        item.addEventListener('click', signIn);
    }
    authState.textContent = '';
    authState.appendChild(item);
}

function CallApiWithAuthorization(endpoint, resp, callback) {
    const headers = new Headers();
    headers.append("Authorization", `Bearer ${resp.accessToken}`);

    const options = {
        method: "POST",
        headers: headers
    };

    fetch(APPLICATION_URL + endpoint, options)
        .then(response => response.json())
        .then(response => callback(response, APPLICATION_URL + endpoint))
        .catch(error => console.log(error));
}


