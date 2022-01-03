class Signup extends HTMLElement {
    constructor(){
        super();
    }
    connectedCallback(){
        this.innerHTML = `
            <div class="container">

            <table style="border:none; background-color:#ffd700">
            <tr>
            <td style="text-align:middle;  width: 800px">
                <form action="https://ch1n9.eu.pythonanywhere.com/ch1n9_signup" method="POST">
                    <input type="email" name="email_address">
                    <input type="hidden" name="signup_page" value="https://tailoryourbim.com/assets/signup/thanks.html" /> 
                    <input type="submit" value="Sign up with email">
                </form>
            </td>
            </tr>
            </table>
            </div>
            `;
    }
}

customElements.define('signup-component', Signup)