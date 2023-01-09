import axios from "axios"
import { useEffect, useState } from "react";
import { useNavigate, useSearchParams } from "react-router-dom"

export const EmailConfirmation = () => {
    let [searchParams, setSearchParams] = useSearchParams();
    const [isLoading, setIsLoading] = useState(false);
    const [success, setSuccess] = useState(false);
    const [failure, setFailure] = useState(false);
    const navigate = useNavigate();
    let id = searchParams.get("id");
    let code = searchParams.get("code");
    console.log(id, code);
    useEffect(()=>{
        if (code && id) {
            setIsLoading(true);
            setSuccess(false);
            setFailure(false);
            axios.post("/api/v1/Account/email-confirmation",
            {
                id: id,
                code: code
            })
            .then(response => {
                setSuccess(true);
                setFailure(false);
            })
            .catch(error => {
                setSuccess(false);
                setFailure(true);
            })
            .then(() => {
                setIsLoading(false);
            });
        }
        
    },[code, id]);
    if (code == null || id == null) {
        return (
            <p>Insufficient data</p>
        );
    } 
    if (isLoading) {
        return (
            <p>Loading</p>
        );
    }
    if (failure) {
        return (
            <p>Verification failed.</p>
        );
    }
    if (success) {
        return (
            <p>Ok</p>
        );
    }
    return (
        <>
            <pre>{id}</pre>
            <pre>{code}</pre>
        </>
    );
}

export default EmailConfirmation;