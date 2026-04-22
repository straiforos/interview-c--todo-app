import React, { ReactNode, useEffect, useState } from 'react';
import { authService } from '../../services/auth.service';

interface HasPermissionProps {
  permission: string;
  children: ReactNode;
  fallback?: ReactNode;
}

export const HasPermission: React.FC<HasPermissionProps> = ({ permission, children, fallback = null }) => {
  const [hasPerm, setHasPerm] = useState(authService.hasPermission(permission));

  useEffect(() => {
    const subscription = authService.currentUser$.subscribe(() => {
      setHasPerm(authService.hasPermission(permission));
    });
    return () => subscription.unsubscribe();
  }, [permission]);

  if (!hasPerm) {
    return <>{fallback}</>;
  }

  return <>{children}</>;
};
