import { Outlet, Link, useNavigate, useNavigation } from 'react-router';
import { authService } from '../services/auth.service';
import { Button } from '@/components/ui/button';
import { LanguageSelect } from './LanguageSelect';
import { useObservable } from '../hooks/useObservable';
import { LogOut, CheckSquare, Loader2, User as UserIcon } from 'lucide-react';
import { useLingui } from '@lingui/react/macro';

export function Layout() {
  const { t } = useLingui();
  const user = useObservable(authService.currentUser$);
  const navigate = useNavigate();
  const navigation = useNavigation();
  const isLoading = navigation.state === 'loading';

  const handleLogout = () => {
    authService.logout();
    navigate('/login');
  };

  if (!user) return <Outlet />;

  return (
    <div className="min-h-screen flex flex-col">
      <header className="border-b bg-background/95 backdrop-blur supports-[backdrop-filter]:bg-background/60 sticky top-0 z-50">
        <div className="container mx-auto px-4 h-16 flex items-center justify-between">
          <Link to="/" className="flex items-center gap-2 font-bold text-xl">
            <CheckSquare className="h-6 w-6 text-primary" />
            <span>TaskFlow</span>
          </Link>

          <div className="flex items-center gap-4">
            <LanguageSelect />

            <Button variant="ghost" size="sm" asChild className="hidden sm:flex">
              <Link to="/profile">
                <UserIcon className="h-4 w-4 mr-2" />
                {user.email}
              </Link>
            </Button>
            
            <Button variant="ghost" size="sm" onClick={handleLogout}>
              <LogOut className="h-4 w-4 mr-2" />
              {t({ id: 'auth.logout', message: 'Logout' })}
            </Button>
          </div>
        </div>
      </header>

      <main className="flex-1 container mx-auto px-4 py-8 relative">
        {isLoading && (
          <div className="absolute inset-0 bg-background/50 flex items-center justify-center z-40">
            <Loader2 className="h-8 w-8 animate-spin text-primary" />
          </div>
        )}
        <Outlet />
      </main>

      <footer className="border-t py-6 bg-muted/50">
        <div className="container mx-auto px-4 text-center text-sm text-muted-foreground">
          © {new Date().getFullYear()} TaskFlow. Built for the DTO Pattern demonstration.
        </div>
      </footer>
    </div>
  );
}
